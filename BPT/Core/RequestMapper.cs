using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPT.Core
{
    public static class RequestMapper
    {
        public static Dictionary<string, string> Mappings = new Dictionary<string, string>()
        {
            {
                "GET /apiext/texts/ja_JP", "loc.json"
            }
        };

        public static (string key, string file) Find(string key)
        {
            if (Mappings.ContainsKey(key))
            {
                return (key, file: Mappings[key]);
            }

            return (key: null, file: null);
        }

        public static bool HasMapping(byte[] bytes, out (string key, string file) mapping)
        {
            foreach (string key in Mappings.Keys)
            {
                if (Encoding.UTF8.GetString(bytes).Split(new string[1] { Environment.NewLine }, StringSplitOptions.None)[0].StartsWith(key))
                {
                    mapping = (key, file: Mappings[key]);
                    return true;
                }
            }

            mapping = (null, null);
            return false;
        }
    }
}
