using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ViazyNetCore.Identity.Validator
{
    public class PasswordHasher : IPasswordHasher<IdentityUser>
    {
        private readonly ViazyIdentityOptions _identityOptions;

        public PasswordHasher(IOptions<ViazyIdentityOptions> viazyIdentityOptions)
        {
            this._identityOptions = viazyIdentityOptions.Value;
        }

        public string HashPassword(IdentityUser user, string password)
        {
            if (user.Password.IsNull())
            {
                user.PasswordSalt = Guid.NewGuid();
                return UserPasswordHelper.EncodePassword(password, user.PasswordSalt, this._identityOptions.UserPasswordFormat);
            }

            return null;
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            if (UserPasswordHelper.CheckPassword(providedPassword, hashedPassword, user.PasswordSalt, this._identityOptions.UserPasswordFormat))
            {
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }
    }
}
