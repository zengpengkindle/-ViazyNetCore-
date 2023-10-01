using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Filter;
using ViazyNetCore.Filter.Descriptor;

namespace ViazyNetCore.Authorization.Controllers
{
    public class ApiController : DynamicControllerBase
    {
        private readonly IApiManager _apiManager;

        public ApiController(IApiManager apiManager)
        {
            this._apiManager = apiManager;
        }

        [HttpPost]
        public List<ApiGroupDescriptor> GetApis()
        {
            return this._apiManager.GetApiDescriptors();
        }
    }
}
