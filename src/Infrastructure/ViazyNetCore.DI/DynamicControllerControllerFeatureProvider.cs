using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using ViazyNetCore.Core.System;

namespace ViazyNetCore.DI
{
    public class DynamicControllerControllerFeatureProvider : ControllerFeatureProvider
    {
        private ISelectController _selectController;

        public DynamicControllerControllerFeatureProvider(ISelectController selectController)
        {
            _selectController = selectController;
        }

        protected override bool IsController(TypeInfo typeInfo)
        {
            return _selectController.IsController(typeInfo);
        }
    }
}
