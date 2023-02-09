using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Domain;

namespace ViazyNetCore.Manage.WebApi.ViewModel
{
    public class RolePagesModel
    {
        public List<PageGroupModel>? Groups { get; internal set; }
        public List<PageSimpleModel>? Pages { get; internal set; }
        public List<long>? CheckedKeys { get; internal set; }
    }
}
