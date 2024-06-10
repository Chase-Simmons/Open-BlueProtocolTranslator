using BPT.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BPT.Models
{
    public class Response
    {
        public string Type;
        public string Etag;
        public string AmzId2;
        public string AmzRequestId;
        public string AmzServerSideEncryption;
        public string AmzMetaSbIv;
        public string AmzMetaSbRawDataSize;
        public string ContentType;
        public string Server;
        public string AcceptRanges;
        public string LastModified;
        public string ContentEncoding;
        public string ContentLength;
        public string Connection;
        public string Date;
        public string EoLogUuid;
        public string EoCacheStatus;

        public Response(string UTF8)
        {
            this.Parse(UTF8);
        }

        public async void Log()
        {
            if (
               string.IsNullOrEmpty(Type) &&
               string.IsNullOrEmpty(Etag) &&
               string.IsNullOrEmpty(AmzId2) &&
               string.IsNullOrEmpty(AmzRequestId) &&
               string.IsNullOrEmpty(AmzServerSideEncryption) &&
               string.IsNullOrEmpty(AmzMetaSbIv) &&
               string.IsNullOrEmpty(AmzMetaSbRawDataSize) &&
               string.IsNullOrEmpty(ContentType) &&
               string.IsNullOrEmpty(Server) &&
               string.IsNullOrEmpty(AcceptRanges) &&
               string.IsNullOrEmpty(LastModified) &&
               string.IsNullOrEmpty(ContentEncoding) &&
               string.IsNullOrEmpty(ContentLength) &&
               string.IsNullOrEmpty(Connection) &&
               string.IsNullOrEmpty(Date) &&
               string.IsNullOrEmpty(EoLogUuid) &&
               string.IsNullOrEmpty(EoCacheStatus)
               )
            {
                return;
            }

            C.Section("Request");

            C.Debug($"[ Type ] : {Type}");
            C.Debug($"[ Etag ] : {Etag}");
            C.Debug($"[ x-amz-id-2 ] : {AmzId2}");
            C.Debug($"[ x-amz-request-id ] : {AmzRequestId}");
            C.Debug($"[ x-amz-server-side-encryption ] : {AmzServerSideEncryption}");
            C.Debug($"[ x-amz-meta-x-sb-iv ] : {AmzMetaSbIv}");
            C.Debug($"[ x-amz-meta-x-sb-rawdatasize ] : {AmzMetaSbRawDataSize}");
            C.Debug($"[ ContentType ] : {ContentType}");
            C.Debug($"[ Server ] : {Server}");
            C.Debug($"[ AcceptRanges ] : {AcceptRanges}");
            C.Debug($"[ LastModified ] : {LastModified}");
            C.Debug($"[ ContentEncoding ] : {ContentEncoding}");
            C.Debug($"[ ContentLength ] : {ContentLength}");
            C.Debug($"[ Connection ] : {Connection}");
            C.Debug($"[ Date ] : {Date}");
            C.Debug($"[ EO-LOG-UUID ] : {EoLogUuid}");
            C.Debug($"[ EO-Cache-Status ] : {EoCacheStatus}");

            C.EndSection();
        }
    }
}
