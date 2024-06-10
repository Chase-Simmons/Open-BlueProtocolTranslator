using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BPT.Core
{
    public static class C
    {
        private static bool _active = false;
        private static bool _isProcessing = false;

        private static Action[] _e2Actions { get; set; }

        public static bool IsProcessing()
        {
            return _isProcessing;
        }

        public static void SetupHandler(X509 X509, Host Host)
        {
            _e2Actions = new Action[] {() => Host.RemoveRedirect(), () => X509.CloseStore()};

            ConsoleEventDelegate handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
        }

        public static void Section(string title, bool bypassDebug = false)
        {
            _isProcessing = true;
            SectionBuilder(title, bypassDebug: bypassDebug);
        }

        public static void EndSection(bool bypassDebug = false)
        {
            _isProcessing = false;
            SectionBuilder(lineBefore: true, bypassDebug: bypassDebug);
        }

        public static void Write(string text, int extraLines = 0, bool lineBefore = false) 
        {
            if (lineBefore)
            {
                Console.WriteLine();
            }

            for (int i = 0; i < extraLines; i++)
            {
                text += "\n";
            }

            Console.WriteLine(text);
        }

        public static void Debug(string text, int extraLines = 0, bool lineBefore = false)
        {
            if (lineBefore)
            {
                Out();
            }

            for (int i = 0; i < extraLines; i++)
            {
                text += "\n";
            }

            Out(text);
        }

        public static void Error(Exception ex)
        {
            Error(ex.Message);
        }

        public static void Error(string message)
        {
            string output = $"[ ERROR ]\n    {message}";
            Out(output);
        }

        private static void SectionBuilder(string message = "", char c = '~', int amount = 90, int extraLines = 1, bool lineBefore = false, bool bypassDebug = false)
        {
            string output = $"{c}{c}";

            if (!string.IsNullOrEmpty(message))
            {
                output += $"[ {message} ]";
            }

            while (output.Length < amount) 
            {
                output += c;
            }

            for (int i = 0; i < extraLines; i++)
            {
                output += "\n";
            }

            if (lineBefore)
            {
                if (!_active && !bypassDebug)
                {
                    Out();
                }
                else
                {
                    Console.WriteLine();
                }
            }

            if (!_active && !bypassDebug)
            {
                Out();
            }
            else
            {
                Console.WriteLine(output);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        private delegate bool ConsoleEventDelegate(int eventType);

        private static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                foreach (Action action in _e2Actions) 
                {
                    action.Invoke();
                }
            }

            Console.WriteLine(eventType);

            return false;
        }

        private static void Out(string message = "")
        {
            if (_active) 
            {
                Console.WriteLine(message);
            }
        }

        public static void EnableDebug()
        {
            _active = true;
        }

        public static bool IsDebugActive()
        {
            return _active;
        }
    }
}
