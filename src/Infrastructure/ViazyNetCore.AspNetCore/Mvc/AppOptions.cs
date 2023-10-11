using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ViazyNetCore
{
    public class AppOptions
    {
        public string ApplicationName { get; set; }
        public AppType AppType { get; set; } = AppType.Controllers;
        public string[] CorUrls { get; set; }
        public bool Tenant { get; set; } = false;
        public bool MiniProfiler { get; set; } = false;

        public IList<Assembly> ApplicationParts { get; } = new List<Assembly>();
        public IList<IApplicationFeatureProvider> FeatureProviders { get; } = new List<IApplicationFeatureProvider>();
    }
    public enum AppType
    {
        Controllers,
        ControllersWithViews,
        MVC
    }
}
