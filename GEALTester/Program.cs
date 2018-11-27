using System;
using GEALTest;

namespace GEALTester
{
    class Program
    {
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
                ButtonEnum button;
                StageEnum wait;
                StageEnum started;

                // ステージ000の開始待ち
                wait = StageEnum.Stage000;
                started = client.StageWait(wait, 10000);
                Console.WriteLine("StageWait({0}) {1}", wait.ToString(), (started == wait) ? "OK" : string.Format("NG({0})", started.ToString()));

                // ステージ001へ移る -> OK
                button = ButtonEnum._00_NextBtn;
                wait = StageEnum.Stage001;
                client.ButtonPush(button);
                started = client.StageWait(wait, 100);
                Console.WriteLine("ButtonPush({0}) StageWait({1}) {2}", button.ToString(), wait.ToString(), (started == wait) ? "OK" : string.Format("NG({0})", started.ToString()));

                // ステージ002へ移る -> NG(Stage003)
                button = ButtonEnum._01_NextBtn;
                wait = StageEnum.Stage003;
                client.ButtonPush(button);
                started = client.StageWait(wait, 100);
                Console.WriteLine("ButtonPush({0}) StageWait({1}) {2}", button.ToString(), wait.ToString(), (started == wait) ? "OK" : string.Format("NG({0})", started.ToString()));
            }
        }
    }
}
