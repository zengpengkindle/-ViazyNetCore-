using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore;

public interface IInjectionModule
{

    Task ConfigureServicesAsync(ServiceConfigurationContext context);

    void ConfigureServices(ServiceConfigurationContext context);
}
