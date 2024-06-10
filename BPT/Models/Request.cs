using BPT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BPT.Core.UTF8Parser;

namespace BPT.Models
{
    public class Request
    {
        public string Type;
        public string Host;
        public string Accept;
        public string AcceptEncoding;
        public string ContentType;
        public string ClientVersion;
        public string RequestedBy;
        public string IsEditor;
        public string EnvAccessToken;
        public string MasterDataType;
        public string ContentLength;
        public string UserAgent;

        /*
          
            GET /apiext/master_nappos?t=1717549826 HTTP/1.1
            Host: masterdata-main.aws.blue-protocol.com
            Accept: 
            Accept-Encoding: deflate, gzip
            Content-Type: application/json
            Client-Version: 1.05.101.794910
            x-requested-by: client
            x-is-editor: false
            x-env-access-token: TsBHDef6MmfAtR4T6FBUk7ym5mqVU3Rh
            MasterDataType: 109
            Content-Length: 0
            User-Agent: BLUEPROTOCOL/++UE4+Release-4.27-CL-0 Windows/10.0.22621.1.256.64bit

         */

        public Request(string UTF8)
        {
            this.Parse(UTF8);
        }

        public async void Log()
        {
            if (
                string.IsNullOrEmpty(Type) &&
                string.IsNullOrEmpty(Host) &&
                string.IsNullOrEmpty(Accept) &&
                string.IsNullOrEmpty(AcceptEncoding) &&
                string.IsNullOrEmpty(ContentType) &&
                string.IsNullOrEmpty(ClientVersion) &&
                string.IsNullOrEmpty(RequestedBy) &&
                string.IsNullOrEmpty(IsEditor) &&
                string.IsNullOrEmpty(EnvAccessToken) &&
                string.IsNullOrEmpty(MasterDataType) &&
                string.IsNullOrEmpty(ContentLength) &&
                string.IsNullOrEmpty(UserAgent)
                )
            {
                return;
            }

            C.Section("Request");

            C.Debug($"[ Type ] : {Type}");
            C.Debug($"[ Host ] : {Host}");
            C.Debug($"[ Accept ] : {Accept}");
            C.Debug($"[ AcceptEncoding ] : {AcceptEncoding}");
            C.Debug($"[ ContentType ] : {ContentType}");
            C.Debug($"[ ClientVersion ] : {ClientVersion}");
            C.Debug($"[ RequestedBy ] : {RequestedBy}");
            C.Debug($"[ IsEditor ] : {IsEditor}");
            C.Debug($"[ EnvAccessToken ] : {EnvAccessToken}");
            C.Debug($"[ MasterDataType ] : {MasterDataType}");
            C.Debug($"[ ContentLength ] : {ContentLength}");
            C.Debug($"[ UserAgent ] : {UserAgent}");

            C.EndSection();
        }
    }
}
