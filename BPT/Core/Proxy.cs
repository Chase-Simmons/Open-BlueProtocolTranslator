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
using System.Xml.Serialization;
using BPT.Models;

namespace BPT.Core
{
    public class Proxy
    {
        private X509 X509 = new X509();
        private Host Host = new Host();
        private TcpListener listener = new TcpListener(IPAddress.Loopback, 443);
        private bool _active = true;

        public void Run()
        {
            X509.Initialize();
            Host.Initialize();

            //C.Section("Open-BPT (Open Blue Protocol Translator)", bypassDebug: true);
            C.Write("Open Blue Protocol...\n");

            listener.Start();

            new Task(() =>
            {
                while (_active)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    new Task(() =>
                    {
                        SslStream Client = new SslStream(client.GetStream(), false);
                        Client.AuthenticateAsServer(X509.Get(), false, false);

                        SslStream AWS = new SslStream(new TcpClient(Host.GetIp(), 443).GetStream(), false);
                        AWS.AuthenticateAsClient("masterdata-main.aws.blue-protocol.com");

                        HandleClientTraffic(Client, AWS);
                        HandleAWSTraffic(AWS, Client);

                    }).Start();
                }

            }).Start();
        }

        private void HandleClientTraffic(SslStream outbound, SslStream target)
        {
            new Task(() =>
            {
                byte[] numArray = new byte[4096];

                while (_active)
                {
                    int count = ValidateCount(outbound, numArray) ?? 0;

                    if (count != 0)
                    {
                        HandleDebugging(numArray, true);

                        if (RequestMapper.HasMapping(numArray, out (string key, string file) mapping))
                        {
                            HandleInterceptedRequest(mapping, outbound);
                            
                            continue;
                        }

                        target.Write(numArray, 0, count);
                    }
                    else
                    {
                        break;
                    }
                }

            }).Start();
        }

        private void HandleAWSTraffic(SslStream outbound, SslStream target)
        {
            new Task(() =>
            {
                byte[] numArray = new byte[4096];

                while (_active)
                {
                    int count = ValidateCount(outbound, numArray) ?? 0;

                    if (count != 0)
                    {
                        HandleDebugging(numArray, false);

                        target.Write(numArray, 0, count);
                    }
                    else
                    {
                        break;
                    }
                }

            }).Start();
        }

        public void HandleInterceptedRequest((string key, string file) mapping, SslStream target)
        {
            C.Debug($@"[ Intercepted Request ] : {mapping.key}");

            string str = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), mapping.file));

            C.Debug($"[ Data Replacement ] : {mapping.file}", extraLines: 1);

            target.Write(Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: " + str.Length.ToString() + "\r\nConnection: keep-alive\r\n\r\n" + str));

            C.Write("Files translated, the program will close in 5 seconds");

            Host.RemoveRedirect();

            //Thread.Sleep(TimeSpan.FromSeconds(5.0));

            X509.CloseStore();

            //C.EndSection(bypassDebug: true);

            Program.Shutdown();
        }

        private List<string> _contents = new List<string>();
        private void HandleDebugging(byte[] bytes, bool outbound)
        {
            if (C.IsDebugActive())
            {
                string strout = Encoding.UTF8.GetString(bytes);

                if (!outbound)
                {
                    if (strout.StartsWith(@"�"))
                    {
                        _contents.Add(strout);
                        C.Debug("\nContent has been logged\n");
                    }
                }

                if (outbound)
                {
                    Task.Run(new Request(strout).Log).Wait();
                }
                else
                {
                    Task.Run(new Response(strout).Log).Wait();
                }
            }
        }

        private int? ValidateCount(SslStream stream, byte[] bytes)
        {
            try
            {
                return stream.Read(bytes, 0, 4096);
            }
            catch
            {
                return null;
            }
        } 
    }
}
