using System;
using System.Runtime.Remoting.Messaging;
using GEALTest;
using GealRsxEnum;

namespace TestProject
{
    class Program
    {
        volatile static bool _keyReaded = false;

        static void MyHandler(IAsyncResult ar)
        {
            AsyncResult asyncResult = (AsyncResult)ar;
            Func<bool, ConsoleKeyInfo> asyncDelegate = (Func<bool, ConsoleKeyInfo>)asyncResult.AsyncDelegate;

            ConsoleKeyInfo keyInfo = asyncDelegate.EndInvoke(asyncResult);
            if (keyInfo.KeyChar  == 'q')
            {
                _keyReaded = true;
            }
        }

        private enum _mode
        {
            Help,
            Record,
            AutoRun,
        };

        static void Main(string[] args)
        {
            var mode = _mode.Help;
            var wait_port = 0x5447; // 21575:"GT";
            var to_host = "127.0.0.1";
            var to_port = 0x7447; // 29767:"Gt"
            var prev_arg = "";
            foreach (var arg in args)
            {
                if (arg.Substring(0, 1) == "-")
                {
                    switch (arg.ToLower())
                    {
                        case "-help":           mode = _mode.Help;      break;
                        case "-record-mode":    mode = _mode.Record;    break;
                        case "-auto-run":       mode = _mode.AutoRun;   break;
                        default:                prev_arg = arg;         break;
                    }
                }
                else
                {
                    var error = false;
                    switch (prev_arg.ToLower())
                    {
                        case "-wait-port":  error = int.TryParse(arg, out wait_port); break;
                        case "-to-host":    to_host = arg;  break;
                        case "-to-port":    error = int.TryParse(arg, out to_port); break;
                        default:            error = true; break;
                    }
                }
            }
            Console.WriteLine("wait_port:{0} to_host:{1} to_port:{2}", wait_port, to_host, to_port);
            TargetToString targetToString = (byte type, ushort id) => {
                var result = "";
                switch (type)
                {
                    case 1: result = ((eGE_BITMAP_ID)id).ToString(); break;
                    case 2: result = ((eGE_FONT_ID)id).ToString(); break;
                    case 3: result = ((eGE_STRING_ID)id).ToString(); break;
                    case 4: result = ((eGE_LANGUAGE_ID)id).ToString(); break;
                    case 5: result = ((eGE_EVENT_ID)id).ToString(); break;
                    case 6: result = ((eGE_BORDER_ID)id).ToString(); break;
                    case 7: result = ((eGE_STAGE_ID)id).ToString(); break;
                    case 8: result = ((eGE_LAYER_ID)id).ToString(); break;
                    case 9: result = ((eGE_WIDGET_ID)id).ToString(); break;
                    default: result = ""; break;
                }
                return result;
            };
            using (var client = new Client(new UDPPort(wait_port, to_host, to_port), new RequestFactory(targetToString)))
            {
                switch (mode)
                {
                    case _mode.Record:
                    {
                        //new Func<bool, ConsoleKeyInfo>(Console.ReadKey).BeginInvoke(true, MyHandler, null);
                        Console.WriteLine("Loop start.");
                        while (!_keyReaded && client.IsOpened)
                        {
                            try
                            {
                                var request = client.Receive();
                                if (request != null)
                                    Console.WriteLine(request.ToLogText());
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("client.port.Receive(): {0}", ex.ToString());
                            }
                        }
                        Console.WriteLine("Loop end.");
                        break;
                    }
                    case _mode.AutoRun:
                    {
                        RequestBase operation;
                        RequestBase wait;
                        RequestBase received;

                        // ステージ000の開始待ち
                        wait = new UGxStageEnter(targetToString,(ushort)eGE_STAGE_ID.eSTGID_Stage000);
                        received = client.Wait(wait, 10000);
                        Console.WriteLine("{0,-10} {1} {2}", "Wait", wait.ToLogText(), wait.Equals(received) ? "OK" : string.Format("NG:{0}", received.ToLogText()));

                        // ステージ001へ移る -> OK
                        operation = new ButtonClick(targetToString, (ushort)eGE_WIDGET_ID.eWGTID_00_NextBtn);
                        client.Operation(operation);
                        Console.WriteLine("{0,-10} {1}", "Operation", operation.ToLogText());
                        wait = new UGxStageEnter(targetToString, (ushort)eGE_STAGE_ID.eSTGID_Stage001);
                        received = client.Wait(wait, 1000);
                        Console.WriteLine("{0,-10} {1} {2}", "Wait", wait.ToLogText(), wait.Equals(received) ? "OK" : string.Format("NG:{0}", received.ToLogText()));

                        // ステージ002へ移る -> NG(Stage003)
                        operation = new ButtonClick(targetToString, (ushort)eGE_WIDGET_ID.eWGTID_01_NextBtn);
                        client.Operation(operation);
                        Console.WriteLine("{0,-10} {1}", "Operation", operation.ToLogText());
                        wait = new UGxStageEnter(targetToString, (ushort)eGE_STAGE_ID.eSTGID_Stage003);
                        received = client.Wait(wait, 1000);
                        Console.WriteLine("{0,-10} {1} {2}", "Wait", wait.ToLogText(), wait.Equals(received) ? "OK" : string.Format("NG:{0}", received.ToLogText()));

                        break;
                    }
                }
            }
        }
    }
}
