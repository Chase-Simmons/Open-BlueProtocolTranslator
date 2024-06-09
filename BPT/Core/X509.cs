using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BPT.Core
{
    public class X509
    {
        private X509Store _store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
        private string _baseLocation = Path.Combine(Directory.GetCurrentDirectory(), "bpmasterdata.pfx");

        private X509Certificate2 _cert { get; set; }


        public void Initialize(string certLocation = null)
        {
            C.Section("X509 Initialization");

            try
            {
                Set(new X509Certificate2(certLocation ?? _baseLocation));

                C.Debug("X509Certificate Info :");
                C.Debug(_cert.ToString(), lineBefore: true);
                C.Debug("X509Certificate successfully setup");

                OpenStore();
            }
            catch (Exception ex)
            {
                C.Debug("X509Certificate failed to setup");
                C.Error(ex);
            }

            C.EndSection();
        }

        private void OpenStore()
        {
            try
            {
                using (_store)
                {
                    _store.Open(OpenFlags.ReadWrite);
                    _store.Add(Get());
                }

                C.Debug($"X509Store Opened: [ NAME: {_store.Name} ]", lineBefore: true);
            }
            catch (Exception ex) 
            {
                C.Debug("X509Store failed to open");
                C.Error(ex);
            }
        }

        public void CloseStore() 
        {
            try
            {
                using (_store)
                {
                    _store.Open(OpenFlags.ReadWrite);
                    _store.Remove(Get());
                }

                C.Debug($"X509Store Closed: [ NAME: {_store.Name} ]", lineBefore: true);
            }
            catch (Exception ex) 
            {
                C.Debug("X509Store failed to close");
                C.Error(ex);
            }
        }

        public bool IsValid()
        {
            return _cert != null;
        }

        public X509Certificate2 Get()
        {
            lock (_cert)
            {
                return _cert;
            }
        }

        private void Set(X509Certificate2 cert)
        {
            lock (cert) 
            {
                _cert = cert;
            }
        }
    }
}
