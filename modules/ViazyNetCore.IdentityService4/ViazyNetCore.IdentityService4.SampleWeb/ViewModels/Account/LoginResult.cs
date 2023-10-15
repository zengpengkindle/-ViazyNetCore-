using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.IdentityService4.SampleWeb.ViewModels
{
    public class LoginResult
    {
        public bool IsRedirect { get; set; }

        public string RedirectUrl { get; set; }
    }
}
