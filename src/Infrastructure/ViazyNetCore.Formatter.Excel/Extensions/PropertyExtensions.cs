using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ViazyNetCore.Formatter.Excel.Extensions
{
    public static class PropertyExtensions
    {
        public static PropertyDescriptor GetPropertyDescriptor(this PropertyInfo propertyInfo)
        {
            return TypeDescriptor.GetProperties(propertyInfo.DeclaringType).Find(propertyInfo.Name, false);
        }

        public static string GetPropertyDisplayName(this PropertyInfo propertyInfo)
        {
            var propertyDescriptor = propertyInfo.GetPropertyDescriptor();
            string displayName = null;
            if (!string.IsNullOrWhiteSpace(propertyInfo.GetCustomAttribute<DisplayAttribute>()?.Name))
            {
                displayName = propertyInfo.GetCustomAttribute<DisplayAttribute>()?.Name;
            }

            if (!string.IsNullOrWhiteSpace(propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName))
            {
                displayName = propertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            }

            return displayName ?? propertyDescriptor.DisplayName ?? propertyDescriptor.Name;
        }

        public static object GetPropertyValue(object src, string propName)
        {
            if (src == null) throw new ArgumentException("Value cannot be null.", "src");
            if (propName == null) throw new ArgumentException("Value cannot be null.", "propName");

            if (propName.Contains("."))
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
            }
            else
            {
                var prop = src.GetType().GetProperty(propName);
                return prop != null ? prop.GetValue(src, null) : null;
            }
        }
    }
}
