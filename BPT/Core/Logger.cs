using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.Core
{
    public static class Logger
    {
        public static void DumpTraffic()
        {
            string curDir = Path.Combine(Directory.GetCurrentDirectory());
            string trafficFile = Path.Combine(curDir, "Traffic.log");

            if (File.Exists(trafficFile))
            {
                File.Delete(trafficFile);
            }

            FileStream fs = File.Create(trafficFile);

            Task.Run(() => Traffic.DumpLog(fs)).Wait();

            fs.Close();
        }
    }
}
