using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BPT.Core
{
    public class Proxy
    {
        private X509 X509 = new X509();
        private Host Host = new Host();
        private TcpListener listener = new TcpListener(IPAddress.Loopback, 443);


        public void Run()
        {
            X509.Initialize();
            Host.Initialize();

            C.Section("Open-BPT (Open Blue Protocol Translator)", bypassDebug: true);
            C.Write("Open Blue Protocol...\n");

            listener.Start();

            new Task(() =>
            {
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    new Task(() =>
                    {
                        SslStream sslStream3 = new SslStream(client.GetStream(), false);
                        sslStream3.AuthenticateAsServer(X509.Get(), false, false);

                        SslStream sslStream4 = new SslStream(new TcpClient(Host.GetIp(), 443).GetStream(), false);
                        sslStream4.AuthenticateAsClient("masterdata-main.aws.blue-protocol.com");

                        ProxyConnection(sslStream3, sslStream4, true);
                        ProxyConnection(sslStream4, sslStream3, false);

                    }).Start();
                }

            }).Start();
        }

        private void ProxyConnection(SslStream src, SslStream output, bool toServer)
        {
            new Task(() =>
            {
                byte[] numArray = new byte[4096];

                while (true)
                {
                    int count;

                    try
                    {
                        count = src.Read(numArray, 0, 4096);
                    }
                    catch
                    {
                        break;
                    }

                    if (count != 0)
                    {
                        if (toServer)
                        {
                            if (Encoding.UTF8.GetString(numArray).Split(new string[1]{Environment.NewLine}, StringSplitOptions.None)[0].StartsWith("GET /apiext/texts/ja_JP"))
                            {
                                string str = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "loc.json"));
                                
                                src.Write(Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: " + str.Length.ToString() + "\r\nConnection: keep-alive\r\n\r\n" + str));

                                C.Write("Files translated, the program will close in 5 seconds");

                                Host.RemoveRedirect();

                                Thread.Sleep(TimeSpan.FromSeconds(5.0));

                                X509.CloseStore();

                                C.EndSection(bypassDebug: true);
                                continue;
                            }
                        }

                        output.Write(numArray, 0, count);
                    }
                    else
                    {
                        break;
                    }
                }

            }).Start();
        }
    }
}
