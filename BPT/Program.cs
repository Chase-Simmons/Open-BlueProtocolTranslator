using BPT.Core;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BPT
{
    public sealed class Program
    {
        private static Proxy Proxy = new Proxy();

        public static int Main(string[] args)
        {
            //C.EnableDebug();

            Proxy.Run();

            while (true)
            {
                Console.ReadKey();
            }
        }
    }
}