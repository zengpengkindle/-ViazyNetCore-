using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Identity.Domain
{
    public class ExternalLoginUserInfo
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; private set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public bool? EmailConfirmed { get; set; }

        public bool? TwoFactorEnabled { get; set; }

        public string ProviderKey { get; set; }

        public ExternalLoginUserInfo([NotNull] string email)
        {
            Email = Check.NotNullOrWhiteSpace(email, nameof(email));
        }
    }
}
