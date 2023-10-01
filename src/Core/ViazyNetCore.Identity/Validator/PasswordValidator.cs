using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.Identity.Validator
{
    public class PasswordValidator : IPasswordValidator<IdentityUser>
    {
        private readonly ViazyIdentityOptions _identityOptions;

        public PasswordValidator(IOptions<ViazyIdentityOptions> options)
        {
            this._identityOptions = options.Value;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
        {
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
