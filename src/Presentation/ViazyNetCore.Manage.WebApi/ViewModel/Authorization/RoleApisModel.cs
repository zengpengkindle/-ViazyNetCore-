using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Filter;

namespace ViazyNetCore.Manage.WebApi.ViewModel
{
    public class RoleApisModel
    {
        public List<ApiGroupDescriptor> Apis { get; internal set; }
        public List<string> CheckedKeys { get; internal set; }
    }
}
