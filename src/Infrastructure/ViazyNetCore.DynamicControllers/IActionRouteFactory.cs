using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ViazyNetCore.DynamicControllers
{
    public interface IActionRouteFactory
    {
        string CreateActionRouteModel(string areaName, string controllerName, ActionModel action);
    }

    internal class DefaultActionRouteFactory : IActionRouteFactory
    {
        private static string GetControllerPreFix(ActionModel action)
        {
            var getValueSuccess = DynamicApplicationConsts.AssemblyDynamicApiOptions
                .TryGetValue(action.Controller.ControllerType.Assembly, out var assemblyDynamicApiOptions);
            if (getValueSuccess && !string.IsNullOrWhiteSpace(assemblyDynamicApiOptions?.ApiPrefix))
            {
                return assemblyDynamicApiOptions.ApiPrefix;
            }

            return DynamicApplicationConsts.DefaultApiPreFix;
        }

        public string CreateActionRouteModel(string areaName, string controllerName, ActionModel action)
        {
            var apiPreFix = GetControllerPreFix(action);
            var routeStr = $"{apiPreFix}/{areaName}/{controllerName}/{action.ActionName}".Replace("//", "/");
            return routeStr;
        }
    }
}
