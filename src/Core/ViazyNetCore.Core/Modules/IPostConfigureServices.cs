using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore;

public interface IPostConfigureServices
{

    Task PostConfigureServicesAsync(ServiceConfigurationContext context);

    void PostConfigureServices(ServiceConfigurationContext context);
}
