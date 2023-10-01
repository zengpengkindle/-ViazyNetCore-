using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.Identity.Validator
{
    public class SecurityStampValidator : SecurityStampValidator<IdentityUser>
    {
        public SecurityStampValidator(IOptions<SecurityStampValidatorOptions> options
            , SignInManager<IdentityUser> signInManager
            , ISystemClock clock
            , ILoggerFactory logger) : base(options, signInManager, clock, logger)
        {
        }

        public override Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            return base.ValidateAsync(context);
        }
    }
}
