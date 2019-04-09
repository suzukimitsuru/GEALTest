using GealRsxEnum;
using GEALTest;
using GEALTest.Request;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace TestProject
{
    class Command
    {
        private enum _mode
        {
            Help,
            Record,
            Run,
            List,
        };

        public static void Main(string[] args)
        {
            var mode = _mode.Help;
            var wait_port = 0x5447; // 21575:"GT"
            var to_host = "localhost";
            var to_port = 0x7447; // 29767:"Gt"
            var record_file = "";
            var record_encoding = new UTF8Encoding(false);
            var prev_arg = "";
            foreach (var arg in args)
            {
                if (arg.Substring(0, 1) == "-")
                {
                    switch (arg.ToLower())
                    {
                        case "-help":   mode = _mode.Help;      break;
                        case "-record": mode = _mode.Record;    break;
                        case "-run":    mode = _mode.Run;       break;
                        case "-list":   mode = _mode.List;      break;
                    }
                }
                else
                {
                    var error = false;
                    switch (prev_arg.ToLower())
                    {
                        case "-wait-port": error = int.TryParse(arg, out wait_port); break;
                        case "-to-host": to_host = arg;  break;
                        case "-to-port": error = int.TryParse(arg, out to_port); break;
                        case "-record": record_file = arg; break;
                    }
                }
                prev_arg = arg;
            }
            var geal_test_assembly = Assembly.GetAssembly(typeof(Client));
            var geal_test_name = geal_test_assembly.GetName().Name;
            var geal_test_version = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(geal_test_assembly, typeof(AssemblyFileVersionAttribute));
            var exe_assembly = Assembly.GetExecutingAssembly();
            var exe_node = Path.GetFileNameWithoutExtension(exe_assembly.Location);
            var exe_path = Path.GetDirectoryName(exe_assembly.Location);
            var exe_version = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(exe_assembly,typeof(AssemblyFileVersionAttribute));
            Console.WriteLine("{0} {1}  server {2}:{3}  client localhost:{4}", geal_test_name, geal_test_version.Version.ToString(), to_host, to_port, wait_port);
            Console.WriteLine();
            using (var client = new Client(new UDPPort(wait_port, to_host, to_port),
                new RequestFactory((byte type, ushort id) => {
                    var id_string = "";
                    switch (type)
                    {
                        case 1: id_string = ((eGE_BITMAP_ID)id).ToString(); break;
                        case 2: id_string = ((eGE_FONT_ID)id).ToString(); break;
                        case 3: id_string = ((eGE_STRING_ID)id).ToString(); break;
                        case 4: id_string = ((eGE_LANGUAGE_ID)id).ToString(); break;
                        case 5: id_string = ((eGE_EVENT_ID)id).ToString(); break;
                        case 6: id_string = ((eGE_BORDER_ID)id).ToString(); break;
                        case 7: id_string = ((eGE_STAGE_ID)id).ToString(); break;
                        case 8: id_string = ((eGE_LAYER_ID)id).ToString(); break;
                        case 9: id_string = ((eGE_WIDGET_ID)id).ToString(); break;
                        default: id_string = ""; break;
                    }
                    var type_string = "";
                    if (id_string.Length > 1)
                    {
                        switch (type)
                        {
                            case 1: type_string = "(ushort)eGE_BITMAP_ID."; break;
                            case 2: type_string = "(ushort)eGE_FONT_ID."; break;
                            case 3: type_string = "(ushort)eGE_STRING_ID."; break;
                            case 4: type_string = "(ushort)eGE_LANGUAGE_ID."; break;
                            case 5: type_string = "(ushort)eGE_EVENT_ID."; break;
                            case 6: type_string = "(ushort)eGE_BORDER_ID."; break;
                            case 7: type_string = "(ushort)eGE_STAGE_ID."; break;
                            case 8: type_string = "(ushort)eGE_LAYER_ID."; break;
                            case 9: type_string = "(ushort)eGE_WIDGET_ID."; break;
                            default: type_string = ""; break;
                        }
                    }
                    return type_string + id_string;
                })))
            {
                switch (mode)
                {
                    case _mode.Help:    _help(exe_node); break;
                    case _mode.Record:  _record(client, record_file, record_encoding); break;
                    case _mode.Run:     _run(client); break;
                    case _mode.List:    _list(); break;
                }
            }
        }
        private static void _help(string name)
        {
            Console.WriteLine("{0} <port-option> <mode-option>", name);
            Console.WriteLine("");
            Console.WriteLine("port-option:");
            Console.WriteLine("  -wait-port <GEALTest Client port>");
            Console.WriteLine("  -to-host <GEALTest Server address>");
            Console.WriteLine("  -to-port <GEALTest Server port>");
            Console.WriteLine("mode-option:");
            Console.WriteLine("  -record <Test Program Filename>");
            Console.WriteLine("  -list");
            Console.WriteLine("  -run");
        }
        private static void _record(Client client, string file, Encoding encoding)
        {
            if (Path.GetFileName(file).Length <= 0)
            {
                Console.WriteLine("Error: Record filename is nothing.");
            }
            else
            {
                var _quit_request = false;
                Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
                {
                    Console.WriteLine("Ctrl+C");
                    _quit_request = true;
                    e.Cancel = true;
                };
                Console.WriteLine("Record: {0}", file);
                try
                {
                    var class_name = Path.GetFileNameWithoutExtension(file);
                    File.WriteAllLines(file, new List<string>() {
                        "using GealRsxEnum;",
                        "using GEALTest;",
                        "using GEALTest.Request;",
                        "using System;",
                        "using System.Collections.Generic;",
                        "",
                        "namespace GEALTestProgram",
                        "{",
                        "\t" + "public class " + class_name,
                        "\t" + "{",
                        "\t\t" + "private Client client;",
                        "\t\t" + "public " + class_name + "(Client client)",
                        "\t\t" + "{",
                        "\t\t\t" + "this.client = client;",
                        "\t\t" + "}",
                        "",
                        "\t\t" + "public void TestRun()",
                        "\t\t" + "{",
                    }, encoding);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("client.port.Receive(): {0}", ex.ToString());
                }
                var assert_count = 1;
                var previous_is_event = false;
                var wait_start = DateTime.Now;
                while (!_quit_request && client.IsOpened)
                {
                    try
                    {
                        var request = client.Receive();
                        if (request != null)
                        {
                            Console.WriteLine(request.ToLogText());
                            if (request.IsMessage)
                            {
                                if (previous_is_event)
                                    File.AppendAllLines(file, new List<string>() {
                                        "\t\t\t\t" + "}, " + (((long)(DateTime.Now - wait_start).TotalMilliseconds) * 2) + "));",
                                    }, encoding);
                                assert_count++;
                                File.AppendAllLines(file, new List<string>() {
                                    "",
                                    "\t\t\t" + "client.Assert(\"ëÄçÏ" + assert_count.ToString("d4") + "\",",
                                    "\t\t\t\t" + "client.Operation(new " + request.ToLogText() + "));",
                                }, encoding);
                                previous_is_event = false;
                            }
                            if (request.IsEvent)
                            {
                                if (!previous_is_event)
                                {
                                    wait_start = DateTime.Now;
                                    File.AppendAllLines(file, new List<string>() {
                                        "\t\t\t" + "client.Assert(\"åüèÿ" + assert_count.ToString("d4") + "\",",
                                        "\t\t\t\t" + "client.Wait(new List<RequestBase>() {"
                                    }, encoding);
                                }
                                File.AppendAllLines(file, new List<string>() {
                                    "\t\t\t\t\t" + "new " + request.ToLogText() + ","
                                }, encoding);
                                if (request is UGxAppFinalize)
                                    _quit_request = true;
                                previous_is_event = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("client.port.Receive(): {0}", ex.ToString());
                    }
                }
                try
                {
                    if (previous_is_event)
                        File.AppendAllLines(file, new List<string>() {
                            "\t\t\t" + "}, " + (((long)(DateTime.Now - wait_start).TotalMilliseconds) * 2) + "));",
                        }, encoding);
                    File.AppendAllLines(file, new List<string>() {
                        "\t\t" + "}",
                        "\t" + "}",
                        "}",
                    }, encoding);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("client.port.Receive(): {0}", ex.ToString());
                }
            }
        }
        private static void _list()
        {
            var classes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace == "GEALTestProgram").ToList();
            foreach (var type in classes)
            {
                Console.WriteLine("{0}", type.Name);
                foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                BindingFlags.Public))
                {
                    Console.WriteLine("{0}.{1}()", type.Name, method.Name);
                }
            }

        }
        private static void _run(Client client)
        {
            var classes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace == "GEALTestProgram").ToList();
            client.TotalStart(classes);
            foreach (var clasz in classes)
            {
                client.ClassStart(clasz);
                var constructor = clasz.GetConstructor(new[] { typeof(Client) });
                var instance = constructor.Invoke(new object[] { client });

                foreach (var method in clasz.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance |
                BindingFlags.Public))
                {
                    client.MethodStart(clasz, method);
                    method.Invoke(instance, new object[] { });
                    client.MethodEnd(clasz, method);
                }
                client.ClassEnd(clasz);
            }
            client.TotalEnd(classes);
        }
    }
}
