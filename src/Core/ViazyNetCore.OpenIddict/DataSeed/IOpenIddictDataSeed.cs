using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.DataSeed
{
    public interface IOpenIddictDataSeed
    {
        Task CreateApplicationAsync([NotNull] string name, [NotNull] string type, [NotNull] string consentType, string displayName, string secret, List<string> grantTypes, List<string> scopes, string clientUri = null, string redirectUri = null, string postLogoutRedirectUri = null, List<string> permissions = null);
    }
}
