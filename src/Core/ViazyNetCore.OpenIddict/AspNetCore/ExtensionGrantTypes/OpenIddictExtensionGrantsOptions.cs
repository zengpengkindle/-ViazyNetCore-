using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.AspNetCore.ExtensionGrantTypes
{
    public class OpenIddictExtensionGrantsOptions
    {

        public Dictionary<string, IExtensionGrant> Grants { get; }

        public OpenIddictExtensionGrantsOptions()
        {
            Grants = new Dictionary<string, IExtensionGrant>();
        }

        public TExtensionGrantType Find<TExtensionGrantType>(string name)
            where TExtensionGrantType : IExtensionGrant
        {
            return (TExtensionGrantType)Grants.FirstOrDefault(x => x.Key == name && x.Value is TExtensionGrantType).Value;
        }
    }
}
