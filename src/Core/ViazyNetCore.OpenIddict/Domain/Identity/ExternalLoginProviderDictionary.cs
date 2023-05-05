using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.Domain;

public class ExternalLoginProviderDictionary : Dictionary<string, ExternalLoginProviderInfo>
{
    /// <summary>
    /// Adds or replaces a provider.
    /// </summary>
    public void Add<TProvider>([NotNull] string name)
        where TProvider : IExternalLoginProvider
    {
        this[name] = new ExternalLoginProviderInfo(name, typeof(TProvider));
    }
}