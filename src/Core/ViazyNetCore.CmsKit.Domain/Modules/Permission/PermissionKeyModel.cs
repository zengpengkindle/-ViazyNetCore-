using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit.Modules
{
    public class PermissionKey
    {
        public string Key { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class PermissionModel
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }
}
