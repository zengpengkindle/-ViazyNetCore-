using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;

namespace ViazyNetCore
{
    /// <summary>
    /// 表示一个程序集类别常量
    /// </summary>
    public static class AssembleTypeConsts
    {
        /// <summary>
        /// Package
        /// </summary>
        public const string Package = "package";
        /// <summary>
        /// ReferenceAssembly
        /// </summary>
        public const string ReferenceAssembly = "referenceassembly";
        /// <summary>
        /// Project
        /// </summary>
        public const string Project = "project";
    }

    /// <summary>
    /// 表示一个运行时的帮助类
    /// </summary>
    public class RuntimeHelper
    {
        /// <summary>
        /// 获取项目程序集，排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包
        /// </summary>
        /// <returns></returns>
        public static IList<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            if(deps==null) return list;
            //var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package" && lib.Type!= "referenceassembly");//排除所有的系统程序集、Nuget下载包
            var libs = deps.CompileLibraries.Where(lib => lib.Type == AssembleTypeConsts.Project);
            foreach(var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch(Exception)
                {
                    // ignored
                }
            }
            return list;
        }

        /// <summary>
        /// 获取指定名称的程序集
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly? GetAssembly(string assemblyName)
        {
            return GetAllAssemblies().FirstOrDefault(assembly => assembly.FullName!.Contains(assemblyName));
        }

        /// <summary>
        /// 获取项目程序集下所有类。
        /// </summary>
        /// <returns></returns>
        public static IList<Type> GetAllTypes()
        {
            var list = new List<Type>();
            foreach(var assembly in GetAllAssemblies())
            {
                var typeInfos = assembly.DefinedTypes;
                foreach(var typeInfo in typeInfos)
                {
                    list.Add(typeInfo.AsType());
                }
            }
            return list;
        }

        /// <summary>
        /// 获取指定项目程序集下所有类。
        /// </summary>
        /// <returns></returns>
        public static IList<Type> GetTypesByAssembly(string assemblyName)
        {
            var list = new List<Type>();
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            var typeInfos = assembly.DefinedTypes;
            foreach(var typeInfo in typeInfos)
            {
                list.Add(typeInfo.AsType());
            }
            return list;
        }


        /// <summary>
        /// 获取具有指定引用类型项目程序集下所有类。
        /// </summary>
        /// <returns></returns>
        public static Type? GetImplementType(string typeName, Type baseInterfaceType)
        {
            return GetAllTypes().FirstOrDefault(t =>
            {
                if(t.Name == typeName &&
                    t.GetTypeInfo().GetInterfaces().Any(b => b.Name == baseInterfaceType.Name))
                {
                    var typeInfo = t.GetTypeInfo();
                    return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType;
                }
                return false;
            });
        }
    }
}
