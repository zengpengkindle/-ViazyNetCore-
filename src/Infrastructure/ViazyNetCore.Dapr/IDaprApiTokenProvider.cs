using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Dapr
{
    public interface IDaprApiTokenProvider
    {
        string GetDaprApiToken();

        string GetAppApiToken();
    }
}
