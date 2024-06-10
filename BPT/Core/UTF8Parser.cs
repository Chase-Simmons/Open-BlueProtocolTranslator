using BPT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BPT.Models.Request;

namespace BPT.Core
{
    public static class UTF8Parser
    {
        public static Request Parse(this Request request, string UTF8)
        {
            if (UTF8.Count(x => x == '?') > 5)
            {
                return null;
            }

            string[] split = UTF8.Split('\n');

            if (split.Length < 12)
            {
                return null;
            }

            request.Type = split[0];
            request.Host = split[1].Replace("Host: ", "");
            request.Accept = split[2].Replace("Accept: ", "");
            request.AcceptEncoding = split[3].Replace("Accept-Encoding: ", "");
            request.ContentType = split[4].Replace("Content-Type: ", "");
            request.ClientVersion = split[5].Replace("Client-Version: ", "");
            request.RequestedBy = split[6].Replace("x-requested-by: ", "");
            request.IsEditor = split[7].Replace("x-is-editor: ", "");
            request.EnvAccessToken = split[8].Replace("x-env-access-token: ", "");
            request.MasterDataType = split[9].Replace("MasterDataType: ", "");
            request.ContentLength = split[10].Replace("Content-Length: ", "");
            request.UserAgent = split[11].Replace("User-Agent: ", "");

            return request;
        }

        public static Response Parse(this Response response, string UTF8)
        {
            string[] split = UTF8.Split('\n');

            if (split.Length < 16)
            {
                return null;
            }

            response.Type = split[0];
            response.Etag = split[1].Replace("Etag: ", "");
            response.AmzId2 = split[2].Replace("x-amz-id-2: ", "");
            response.AmzRequestId = split[3].Replace("x-amz-request-id: ", "");
            response.AmzServerSideEncryption = split[4].Replace("x-amz-server-side-encryption: ", "");
            response.AmzMetaSbIv = split[5].Replace("x-amz-meta-x-sb-iv: ", "");
            response.AmzMetaSbRawDataSize = split[6].Replace("x-amz-meta-x-sb-rawdatasize: ", "");
            response.ContentType = split[7].Replace("Content-Type: ", "");
            response.Server = split[8].Replace("Server: ", "");
            response.AcceptRanges = split[9].Replace("Accept-Ranges: ", "");
            response.LastModified = split[10].Replace("Last-Modified: ", "");
            response.ContentEncoding = split[11].Replace("Content-Encoding: ", "");
            response.ContentLength = split[12].Replace("Content-Length: ", "");
            response.Connection = split[13].Replace("Connection: ", "");
            response.Date = split[14].Replace("Date: ", "");
            response.EoLogUuid = split[15].Replace("EO-LOG-UUID: ", "");
            response.EoCacheStatus = split[16].Replace("EO-Cache-Status: ", "");

            return response;
        }
    }
}
