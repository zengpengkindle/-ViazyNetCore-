using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Auth.Authorization.ViewModels
{
    public class UpdateBmsRoleArgs
    {
        public BmsRole Item { get; set; }

        public string[] Keys { get; set; }
    }
}
