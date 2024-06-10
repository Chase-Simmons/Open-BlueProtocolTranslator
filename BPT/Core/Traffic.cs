using BPT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BPT.Core
{
    public static class Traffic
    {
        public static Dictionary<Guid, List<byte[]>> Items = new Dictionary<Guid, List<byte[]>>() { };

        public static void Record(Guid id, byte[] data)
        {
            if (!Items.Keys.Contains(id))
            {
                Items.Add(id, new List<byte[]> { data });
            }
            else
            {
                Items[id].Add(data);
            }
        }

        public static void DumpConsole()
        {
            lock (Items)
            {
                for (int index = 0; index < Items.Count; index++)
                {
                    Guid key = Items.Keys.ElementAt(index);
                    List<byte[]> trafficLog = Items.Values.ElementAt(index);

                    if (trafficLog.Count >= 2)
                    {
                        string strUft8_1 = Encoding.UTF8.GetString(trafficLog[0]);

                        new Request(strUft8_1).Log(key.ToString());

                        string fullResponse = "";
                        for (int i = 1; i < trafficLog.Count; i++)
                        {
                            try
                            {
                                fullResponse += Encoding.UTF8.GetString(trafficLog[i]);
                            }
                            catch { }
                        }

                        C.Section($"Response [ {key} ]");
                        C.Debug(fullResponse);
                        C.EndSection();
                    }
                }
            }
        }

        public static void DumpLog(FileStream fs)
        {
            lock (Items)
            {
                string output = "";

                for (int index = 0; index < Items.Count; index++)
                {
                    Guid key = Items.Keys.ElementAt(index);
                    List<byte[]> trafficLog = Items.Values.ElementAt(index);

                    if (trafficLog.Count >= 2)
                    {
                        string strUft8_1 = Encoding.UTF8.GetString(trafficLog[0]);
                        Request request = new Request(strUft8_1);

                        output += $"\n~~[ Request ]~[ {key} ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~n\n";
                        output += ($"[ Type ] : {request.Type}\n");
                        output += ($"[ Host ] : {request.Host}\n");
                        output += ($"[ Accept ] : {request.Accept}\n");
                        output += ($"[ AcceptEncoding ] : {request.AcceptEncoding}\n");
                        output += ($"[ ContentType ] : {request.ContentType}\n");
                        output += ($"[ ClientVersion ] : {request.ClientVersion}\n");
                        output += ($"[ RequestedBy ] : {request.RequestedBy}\n");
                        output += ($"[ IsEditor ] : {request.IsEditor}\n");
                        output += ($"[ EnvAccessToken ] : {request.EnvAccessToken}\n");
                        output += ($"[ MasterDataType ] : {request.MasterDataType}\n");
                        output += ($"[ ContentLength ] : {request.ContentLength}\n");
                        output += ($"[ UserAgent ] : {request.UserAgent}\n");

                        output += "\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";

                        string fullResponse = "";
                        for (int i = 1; i < trafficLog.Count; i++)
                        {
                            try
                            {
                                fullResponse += Encoding.UTF8.GetString(trafficLog[i]);
                            }
                            catch { }
                        }

                        output += $"~~[ Response ]~[ {key} ]~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n\n";
                        output += fullResponse;
                        output += "\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
                    }
                }

                fs.Write(Encoding.ASCII.GetBytes(output), 0, output.Length);
            }
        }
    }
}
