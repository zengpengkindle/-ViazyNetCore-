using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.OpenIddict.AspNetCore.ExtensionGrantTypes
{
    public interface IExtensionGrant
    {

        string Name { get; }

        Task<IActionResult> HandleAsync(ExtensionGrantContext context);
    }
}
