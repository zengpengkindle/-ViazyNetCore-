using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace ViazyNetCore.Swagger
{
    public class ProduceResponseTypeModelProvider : IApplicationModelProvider
    {
        public int Order => 0;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                foreach (ActionModel action in controller.Actions)
                {
                    if (!action.Filters.Any(e => (e is ProducesResponseTypeAttribute producesResponseType) && producesResponseType.StatusCode == StatusCodes.Status200OK))
                    {
                        //跳过带有ApiIgnoreAttribute的action
                        //if (action.Attributes.Any(f => (f is ApiIgnoreAttribute)))
                        //{
                        //    continue;
                        //}

                        if (action.ActionMethod.ReturnType != null)
                        {
                            Type type = typeof(DefaultApiResponseModel<>);

                            //忽略原先旧接口返回类型,后续可移除
                            var returnType = action.ActionMethod.ReturnType;
                            if (returnType.GetInterface("IActionResult") != null)
                            {
                                action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status200OK));
                                continue;
                            }

                            if (returnType == typeof(Task))
                            {
                                action.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status200OK));
                                continue;
                            }

                            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == type)
                            {
                                action.Filters.Add(new ProducesResponseTypeAttribute(returnType, StatusCodes.Status200OK));
                            }

                            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                            {
                                returnType = returnType.GetGenericArguments()!.First();
                            }
                            //type = type.MakeGenericType(returnType);

                            action.Filters.Add(new ProducesResponseTypeAttribute(returnType, StatusCodes.Status200OK));
                        }
                        else
                        {
                            //var type = typeof(DefaultApiResponseModel);
                            //action.Filters.Add(new ProducesResponseTypeAttribute(type, StatusCodes.Status200OK));
                        }
                    }
                }
            }
        }
    }
}
