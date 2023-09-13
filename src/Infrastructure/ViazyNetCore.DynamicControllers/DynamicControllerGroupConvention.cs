using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.DynamicControllers
{
    public class DynamicControllerGroupConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.Attributes?.Count > 0)
            {
                foreach (var attribute in controller.Attributes)
                {
                    if (attribute is AreaAttribute area)
                    {
                        if (controller.ApiExplorer.GroupName.IsNull())
                        {
                            controller.ApiExplorer.GroupName = area.RouteValue?.ToLower();
                        }
                        break;
                    }
                    else if (attribute is DynamicApiAttribute dynamicApi)
                    {
                        if (controller.ApiExplorer.GroupName.IsNull())
                        {
                            controller.ApiExplorer.GroupName = dynamicApi.Area?.ToLower();
                        }
                        break;
                    }
                }
            }
        }
    }
}
