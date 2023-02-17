using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ViazyNetCore.Auth.Authorization.ViewModels
{
    public class PermissionRouterModel
    {
        public string Path { get; set; }

        [JsonIgnore]
        public string Id { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string? Name { get; set; }

        public PermissionRouteMeta Meta { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<PermissionRouterModel>? Children { get; set; }
    }

    public class PermissionRouteMeta
    {
        public string Title { get; set; }

        public string Icon { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Rank { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public List<string>? Roles { get; set; }
    }
}
