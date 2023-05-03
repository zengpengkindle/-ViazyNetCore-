using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.AspNetCore
{
    public class AuthorizeViewModel
    {
        public string ApplicationName { get; set; }

        public string Scope { get; set; }
    }

}
