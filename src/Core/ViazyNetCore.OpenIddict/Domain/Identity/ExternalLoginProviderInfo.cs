using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.OpenIddict.Domain;

public class ExternalLoginProviderInfo
{
    public string Name { get; }

    public Type Type
    {
        get => _type;
        set => _type = Check.NotNull(value, nameof(value));
    }
    private Type _type;

    public ExternalLoginProviderInfo(
        [NotNull] string name,
        [NotNull] Type type)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        Type = Check.AssignableTo<IExternalLoginProvider>(type, nameof(type));
    }
}
