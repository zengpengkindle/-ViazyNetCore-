using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OSS
{
    public class ItemMeta
    {
        public string ObjectName { get; set; }

        public long Size { get; set; }

        public DateTime LastModified { get; set; }

        public string ETag { get; set; }

        public string ContentType { get; set; }

        public bool IsEnableHttps { get; set; }

        public Dictionary<string, string> MetaData { get; set; }
    }
}
