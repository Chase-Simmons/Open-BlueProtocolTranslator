using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BPT.Core
{
    public class Host
    {
        private string _hostsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32/drivers/etc/hosts");

        private const string _ip = "127.0.0.1";
        private const string _address = "masterdata-main.aws.blue-protocol.com";

        private string _hostIp { get; set; }
        private string _redirect { get; set; }  

        public void Initialize()
        {
            try
            {
                C.Section("Host Initialization");

                SetRedirect();

                if (File.ReadAllText(_hostsFile).Contains(_redirect))
                {
                    RemoveRedirect();
                }

                SetHostIp();
                AddRedirect();

            }
            catch (Exception ex)
            {
                C.Debug("Host failed to setup");
                C.Error(ex);
            }

            C.EndSection();
        }

        public string GetIp()
        {
            return _hostIp;
        }

        private void SetHostIp()
        {
            _hostIp = Dns.GetHostEntry("masterdata-main.aws.blue-protocol.com").AddressList[0].ToString();

            C.Debug($"Host IP set : {_hostIp}");
        }

        private void SetRedirect()
        {
            _redirect = $"{_ip} {_address}";

            C.Debug($"Redirect set : {_redirect}");
        }

        private void AddRedirect()
        {
            bool flag = true;

            using (FileStream input = new FileStream(_hostsFile, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(input))
                {
                    input.Position = input.Length - 1L;
                    if (binaryReader.Read() == 10)
                    {
                        flag = false;
                    }

                    binaryReader.Dispose();
                }

                input.Dispose();
            }

            string contents = flag ? Environment.NewLine + "127.0.0.1 masterdata-main.aws.blue-protocol.com" : "127.0.0.1 masterdata-main.aws.blue-protocol.com";

            File.AppendAllText(_hostsFile, contents);
        }

        public void RemoveRedirect()
        {
            File.WriteAllText(_hostsFile, File.ReadAllText(_hostsFile).Replace("127.0.0.1 masterdata-main.aws.blue-protocol.com", ""));
        }
    }
}
