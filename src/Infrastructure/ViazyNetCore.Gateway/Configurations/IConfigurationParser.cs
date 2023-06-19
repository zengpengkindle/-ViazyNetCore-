using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Gateway.Configurations
{
    public interface IConfigurationParser
    {
        IDictionary<string, string> Parse(Stream input, string initialContext);
    }
}
