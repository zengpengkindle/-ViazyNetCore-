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
            if (user.Password == null)
            {
                return Task.FromResult(IdentityResult.Success);
            }

            return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "400", Description = "密码错误" }));
        }
    }
}
