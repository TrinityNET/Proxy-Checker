using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Cheeky
{
    class Program
    {
        public static List<string> Proxies = new List<string>();
        public static List<string> WorkingProxies = new List<string>();
        public static List<string> NonWorkingProxies = new List<string>();

        static void Watermark()
        {
            SetColour("Red");
            Console.WriteLine("               ██████╗██╗  ██╗███████╗███████╗██╗  ██╗██╗   ██╗");
            Console.WriteLine("              ██╔════╝██║  ██║██╔════╝██╔════╝██║ ██╔╝╚██╗ ██╔╝");
            Console.WriteLine("              ██║     ███████║█████╗  █████╗  █████╔╝  ╚████╔╝");
            Console.WriteLine("              ██║     ██╔══██║██╔══╝  ██╔══╝  ██╔═██╗   ╚██╔╝");
            Console.WriteLine("              ╚██████╗██║  ██║███████╗███████╗██║  ██╗   ██║");
            Console.WriteLine("               ╚═════╝╚═╝  ╚═╝╚══════╝╚══════╝╚═╝  ╚═╝   ╚═╝");
            Console.WriteLine("                                                                                               ");
            SetColour("White");
        }

        static void SetColour(string colour)
        {
            switch (colour)
            {
                case "Red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "White":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "Lime":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToLongDateString();

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
                goto Save;
            }

        Save:
            if (!Directory.Exists($"Logs\\{date}"))
            {
                Directory.CreateDirectory($"Logs\\{date}");
            }
            File.WriteAllLines($"Logs\\{date}\\Working.txt", WorkingProxies);
            File.WriteAllLines($"Logs\\{date}\\Proxies.txt", Proxies);

        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            Watermark();
            if (string.IsNullOrEmpty(args.ToString()) || args.Length <= 0)
            {
                SetColour("Red");
                Console.WriteLine("Please drag your proxy list onto the exe!");
                Console.Read();
            }
            else
            {
                foreach(string line in File.ReadAllLines(args[0]))
                {
                    var found = line.Split(':');
                    var proxy = found[0].ToString();
                    var port = found[1].ToString();
                    Proxies.Add(proxy + ":" + port);
                    Console.Title = $"Cheeky Proxy Checker | Loaded: {Proxies.Count.ToString()} proxies";
                }

                SetColour("Lime");
                Console.WriteLine(" [!] Press ENTER at any time to save progress and exit program!\n");

                Task.Run(() => Check());

                Console.Read();
            }
        }

        static void Check()
        {
            for (int i = 0; i < Proxies.Count; i++)
            {
                var proxy = Proxies[i];

                int amountchecked = 0;
                var ip = proxy.Split(':')[0];
                var port = proxy.Split(':')[1];
                var ping = new Ping();
                var reply = ping.Send(ip);

                if (reply.Status == IPStatus.Success)
                {
                    JObject jobject = JObject.Parse(new WebClient().DownloadString("http://ip-api.com/json/" + ip));
                    string country = (string)jobject["regionName"];
                    amountchecked++;
                    SetColour("Lime");
                    Console.WriteLine($" [+] {country} | {proxy} | ({reply.RoundtripTime}ms)");
                    WorkingProxies.Add(proxy);
                    Console.Title = $"Cheeky Proxy Checker | Loaded: {Proxies.Count.ToString()} proxies | Working proxies: {WorkingProxies.Count} | Dead proxies: {NonWorkingProxies.Count}";
                }
                else
                {
                    JObject jobject = JObject.Parse(new WebClient().DownloadString("http://ip-api.com/json/" + ip));
                    string country = (string)jobject["regionName"];
                    amountchecked++;
                    SetColour("Red");
                    Console.WriteLine($" [-] {country} | {proxy} | No response!");
                    NonWorkingProxies.Add(proxy);
                    Console.Title = $"Cheeky Proxy Checker | Loaded: {Proxies.Count.ToString()} proxies | Working proxies: {WorkingProxies.Count} | Dead proxies: {NonWorkingProxies.Count}";
                }
            }
        }
    }
}
