using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.DynamicControllers;

namespace ViazyNetCore.Core.System
{
    public interface ISelectController
    {
        bool IsController(Type type);
    }


    internal class DefaultSelectController : ISelectController
    {
        public bool IsController(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (!typeof(IDynamicController).IsAssignableFrom(type) ||
                !typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
            {
                return false;
            }

            var attr = typeInfo.GetAttribute<DynamicApiAttribute>();

            if (attr == null)
            {
                return false;
            }

            if (typeInfo.GetAttribute<NonDynamicApiAttribute>() != null)
            {
                return false;
            }

            return true;
        }
    }
}
