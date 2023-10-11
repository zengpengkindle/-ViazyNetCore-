using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.CmsKit.ViewModels
{
    public class PermissionApiUpdateDto
    {
        public string PermissionKey { get; set; }

        public List<ApiItemDto> Apis { get; set; }
    }

    public class ApiItemDto
    {
        public string Path { get; set; }

        public string HttpMethod { get; set; }
    }
}
