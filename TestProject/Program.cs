using System;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using GEALTest;

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

        static void Main(string[] args)
        {
            var wait_port = 0x5447; // 21575:"GT";
            var to_host = "127.0.0.1";
            var to_port = 0x7447; // 29767:"Gt"
            foreach (var arg in args)
            {
                if (arg.Substring(0, 1) == "-")
                {
                }
                else
                {
                    if (to_host.Length > 0)
                        int.TryParse(arg, out to_port);
                    else
                        to_host = arg;
                }
            }
            Console.WriteLine("wait_port:{0} to_host:{1} to_port:{2}", wait_port, to_host, to_port);
            using (var client = new Client(new UDPPort(wait_port, to_host, to_port)))
            {
                new Func<bool, ConsoleKeyInfo>(Console.ReadKey).BeginInvoke(true, MyHandler, null);
                Console.WriteLine("Loop start.");
                while (!_keyReaded && client.port.IsOpened)
                {
                    try
                    {
                        var received = client.port.Receive();
                        if (received.Length > 0)
                        {
                            RequestBase request = (received.Length >= (6 + 4)) ? new RequestParameter(received) : new RequestBase(received);
                            Console.WriteLine(request.ToString((TargetTypeEnum type, ushort id) => {
                                var result = "";
                                switch (type)
                                {
                                    case TargetTypeEnum.tteBITMAP: result = ((eGE_BITMAP_ID)id).ToString(); break;
                                    case TargetTypeEnum.tteFONT: result = ((eGE_FONT_ID)id).ToString(); break;
                                    case TargetTypeEnum.tteSTRING: result = ((eGE_STRING_ID)id).ToString(); break;
                                    case TargetTypeEnum.tteLANGUAGE: result = ((eGE_LANGUAGE_ID)id).ToString(); break;
                                    case TargetTypeEnum.tteEVENT: result = ((eGE_EVENT_ID)id).ToString(); break;
                                    case TargetTypeEnum.tteBORDER: result = ((eGE_BORDER_ID)id).ToString(); break;
                                    case TargetTypeEnum.tteSTAGE: result = ((eGE_STAGE_ID)id).ToString(); break;
                                    case TargetTypeEnum.tteLAYER: result = ((eGE_LAYER_ID)id).ToString(); break;
                                    case TargetTypeEnum.tteWIDGET: result = ((eGE_WIDGET_ID)id).ToString(); break;
                                    default:                        result = ""; break;
                                }
                                return result;
                            }));
                            //uint started = BitConverter.ToUInt16(received, 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("client.port.Receive(): {0}", ex.ToString());
                    }
                }
                Console.WriteLine("Loop end.");


                uint button;
                uint wait;
                uint started;

                // ステージ000の開始待ち
                wait = (uint)eGE_STAGE_ID.eSTGID_Stage000;
                started = client.StageWait(wait, 10000);
                Console.WriteLine("StageWait({0}) {1}", wait.ToString(), (started == wait) ? "OK" : string.Format("NG({0})", started.ToString()));

                // ステージ001へ移る -> OK
                button = (uint)eGE_WIDGET_ID.eWGTID_00_NextBtn;
                wait = (uint)eGE_STAGE_ID.eSTGID_Stage001;
                client.ButtonPush(button);
                started = client.StageWait(wait, 100);
                Console.WriteLine("ButtonPush({0}) StageWait({1}) {2}", button.ToString(), wait.ToString(), (started == wait) ? "OK" : string.Format("NG({0})", started.ToString()));

                // ステージ002へ移る -> NG(Stage003)
                button = (uint)eGE_WIDGET_ID.eWGTID_01_NextBtn;
                wait = (uint)eGE_STAGE_ID.eSTGID_Stage003;
                client.ButtonPush(button);
                started = client.StageWait(wait, 100);
                Console.WriteLine("ButtonPush({0}) StageWait({1}) {2}", button.ToString(), wait.ToString(), (started == wait) ? "OK" : string.Format("NG({0})", started.ToString()));
            }
        }
    }
}
