using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class XTypeExtensions
    {
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// 返回一个类型的默认值。
        /// </summary>
        /// <param name="type">值类型或引用类型。</param>
        /// <returns>类型的默认值。</returns>
        public static object? GetDefaultValue(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            var typeInfo = type.GetTypeInfo();
            var dva = typeInfo.GetAttribute<ComponentModel.DefaultValueAttribute>();
            if (dva != null)
            {
                if (dva.Value != null) return type.Parse(dva.Value);
                return dva.Value;
            }

            return typeInfo.IsValueType && !(typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                ? Activator.CreateInstance(type)
                : null;
        }


        /// <summary>
        /// 确定是否可以将此类型的实例分配给
        /// 实例 <typeparamref name="TTarget"></typeparamref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/>.
        /// </summary>
        /// <typeparam name="TTarget">Target type</typeparam> (as reverse).
        public static bool IsAssignableTo<TTarget>([NotNull] this Type type)
        {
            Check.NotNull(type, nameof(type));

            return type.IsAssignableTo(typeof(TTarget));
        }

        /// <summary>
        /// 确定是否可以将此类型的实例分配给
        /// 实例 <paramref name="targetType"></paramref>.
        ///
        /// Internally uses <see cref="Type.IsAssignableFrom"/> (as reverse).
        /// </summary>
        /// <param name="type">this type</param>
        /// <param name="targetType">Target type</param>
        public static bool IsAssignableTo([NotNull] this Type type, [NotNull] Type targetType)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(targetType, nameof(targetType));

            return targetType.IsAssignableFrom(type);
        }

        /// <summary>
        /// 判断类型是否为匿名类型。
        /// </summary>
        /// <param name="type">数据类型。</param>
        /// <returns>如果为匿名类型返回 true，否则返回 false。</returns>
        public static bool IsAnonymous(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            var typeInfo = type.GetTypeInfo();
            return !typeInfo.IsPublic
               && typeInfo.IsSealed
               && typeInfo.GetCustomAttributes(typeof(Runtime.CompilerServices.CompilerGeneratedAttribute), false).Length > 0
               && typeInfo.Name.Contains("AnonymousType");
        }

        /// <summary>
        /// 判断一个类型是否为可空类型。
        /// </summary>
        /// <param name="type">需要判断的类型。</param>
        /// <returns>如果为 true 则是一个可空类型，否则为 false。</returns>
        public static bool IsNullable(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// 尝试返回指定可以为 null 的类型的基础类型参数。
        /// </summary>
        /// <param name="type">需要判断的类型。</param>
        /// <returns>如果 <paramref name="type"/> 参数为关闭的泛型可以为 null 的类型，则为 <paramref name="type"/> 参数的类型变量，否则直接返回 <paramref name="type"/>。</returns>
        public static Type GetUnderlyingType(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// 获取一个值，指示当前类型是否为简单类型。
        /// <para>1、若 <paramref name="type"/> 为可空类型则取其基础类型进行判断。</para>
        /// <para>2、简单类型包括：<see cref="bool"/>、<see cref="byte"/>、<see cref="sbyte"/>、<see cref="short"/>、<see cref="ushort"/>、<see cref="int"/>、<see cref="uint"/>、<see cref="long"/>、<see cref="ulong"/>、<see cref="IntPtr"/>、<see cref="UIntPtr"/>、<see cref="char"/>、<see cref="double"/>、<see cref="float"/>、<see cref="decimal"/>、<see cref="DateTime"/>、<see cref="DateTimeOffset"/>、<see cref="TimeSpan"/>、<see cref="Guid"/>、<see cref="string"/> 和 <see cref="Enum"/>。</para>
        /// </summary>
        /// <param name="type">需要判断的类型。</param>
        /// <returns>如果为简单类型返回 true，否则返回 false。</returns>
        public static bool IsSimpleType(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (type == typeof(string)) return true;
            if (type.IsClass) return false;
            type = type.GetUnderlyingType();

            return type.IsEnum
                || type.GetTypeInfo().IsPrimitive
                    || type.Equals(typeof(Guid))
                    || type.Equals(typeof(DateTime))
                    || type.Equals(typeof(decimal))
                    || type.Equals(typeof(DateTimeOffset))
                    || type.Equals(typeof(TimeSpan));
        }

        /// <summary>
        /// 获取一个值，指示当前类型是否为复杂类型。
        /// </summary>
        /// <param name="type">需要判断的类型。</param>
        /// <returns>如果为复杂类型返回 true，否则返回 false。</returns>
        public static bool IsComplexType(this Type type)
        {
            return !typeof(Collections.IEnumerable).IsAssignableFrom(type)
                    && !type.IsSimpleType();
            //                    && !ComponentModel.TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string));
        }

        /// <summary>
        /// 获取一个值，指示当前类型是否为数据库类型。
        /// <para>1、若 <paramref name="type"/> 为可空类型则取其基础类型进行判断。</para>
        /// <para>2、数据库类型包括符合 <see cref="IsSimpleType(Type)"/> 判定的类型，或者是一个<see cref="byte"/>[] 类型。</para>
        /// </summary>
        /// <param name="type">需要判断的类型。</param>
        /// <returns>如果为数据库类型返回 true，否则返回 false。</returns>
        public static bool IsDbType(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            return type.IsSimpleType() || type == typeof(byte[]);
        }

        //public static T CreateInstance<T>(this Type type)
        //{
        //    //Array.CreateInstance
        //    return (T)Activator.CreateInstance(type);
        //}


        /// <summary>
        /// 将指定的值转换为当前类型。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <param name="value">实例。</param>
        /// <returns>类型转换的实例。</returns>
        public static object? Parse(this Type type, object value)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            if (value is null || Equals(value, DBNull.Value)) return type.GetDefaultValue();

            var realType = type.GetUnderlyingType();
            var typeInfo = realType.GetTypeInfo();

            if (typeInfo.IsInstanceOfType(value)) return value;

            if (realType == typeof(bool)) return (new[] { "true", "1", "checked", "yes", "selected", "ok", "是", "校验" }).Contains(value.ToString(), StringComparer.OrdinalIgnoreCase);

            if (realType == typeof(Guid))
            {
                if (value is byte[] v) return new Guid(v);
                return new Guid(value.ToString());
            }
            if (realType == typeof(TimeSpan))
            {
                if (value is long @int) return new TimeSpan(@int);
                return TimeSpan.Parse(value.ToString());
            }
            if (realType == typeof(DateTimeOffset))
            {
                if (value is DateTime dt) return new DateTimeOffset(dt);
                if (value is long dv) return new DateTimeOffset(new DateTime(dv));
                return DateTimeOffset.Parse(value.ToString()!);
            }
            if (typeInfo.IsEnum)
            {
                if (value is string) return Enum.Parse(realType, Convert.ToString(value), true);
                return Enum.ToObject(realType, value);
            }

            if (realType == typeof(Uri)) return new Uri(value.ToString());

            if (typeof(Collections.IList).IsAssignableFrom(type) && value is Collections.IList source)
            {
                if (type.IsArray)
                {
                    var array = Array.CreateInstance(type.GetElementType(), source.Count);
                    source.CopyTo(array, 0);
                    return array;
                }
                else
                {
                    var target = (Collections.IList)Activator.CreateInstance(type);
                    foreach (var item in source)
                    {
                        target.Add(item);
                    }

                    return target;
                }
            }

            if (value is string sv)
            {
                return (Type.GetTypeCode(realType)) switch
                {
                    TypeCode.Byte => byte.Parse(sv),
                    TypeCode.Char => char.Parse(sv),
                    TypeCode.DateTime => DateTime.Parse(sv),
                    TypeCode.Decimal => decimal.Parse(sv),
                    TypeCode.Double => double.Parse(sv),
                    TypeCode.Int16 => short.Parse(sv),
                    TypeCode.Int32 => int.Parse(sv),
                    TypeCode.Int64 => long.Parse(sv),
                    TypeCode.SByte => sbyte.Parse(sv),
                    TypeCode.Single => float.Parse(sv),
                    TypeCode.String => sv,
                    TypeCode.UInt16 => ushort.Parse(sv),
                    TypeCode.UInt32 => uint.Parse(sv),
                    TypeCode.UInt64 => ulong.Parse(sv),
                    _ => throw new InvalidCastException($"Can't convert type '{value.GetType().GetTypeInfo().FullName}' to '{typeInfo.FullName}' type."),
                };
            }

            return Type.GetTypeCode(realType) switch
            {
                TypeCode.Byte => Convert.ToByte(value),
                TypeCode.Char => Convert.ToChar(value),
                TypeCode.DateTime => value is long lv ? new DateTime(lv) : Convert.ToDateTime(value),
                TypeCode.Decimal => Convert.ToDecimal(value),
                TypeCode.Double => Convert.ToDouble(value),
                TypeCode.Int16 => Convert.ToInt16(value),
                TypeCode.Int32 => Convert.ToInt32(value),
                TypeCode.Int64 => Convert.ToInt64(value),
                TypeCode.SByte => Convert.ToSByte(value),
                TypeCode.Single => Convert.ToSingle(value),
                TypeCode.String => value.ToString(),
                TypeCode.UInt16 => Convert.ToUInt16(value),
                TypeCode.UInt32 => Convert.ToUInt32(value),
                TypeCode.UInt64 => Convert.ToUInt64(value),
                TypeCode.Empty => throw new NotImplementedException(),
                TypeCode.Object => throw new NotImplementedException(),
                TypeCode.DBNull => throw new NotImplementedException(),
                TypeCode.Boolean => throw new NotImplementedException(),
                _ => throw new InvalidCastException($"Can't convert type '{value.GetType().GetTypeInfo().FullName}' to '{typeInfo.FullName}' type."),
            };
        }

        /// <summary>
        /// 检索应用于类型成员的自定义属性。参数指定成员、要搜索的自定义属性的类型以及是否搜索成员的祖先。
        /// </summary>
        /// <typeparam name="T">要搜索的自定义属性的类型或基类型，仅支持接口和继承于 <see cref="Attribute"/> 的类型。</typeparam>
        /// <param name="member">一个从 <see cref="MemberInfo"/> 类派生的对象，该类描述类的构造函数、事件、字段、方法或属性成员。</param>
        /// <param name="inherit">如果为 true，则指定还在 <paramref name="member"/> 的祖先中搜索自定义属性。</param>
        /// <returns>一个引用，指向应用于 <paramref name="member"/> 的 <typeparamref name="T"/> 类型的单个自定义属性；如果没有此类属性，则为 null。</returns>
        public static T GetAttribute<T>(this ICustomAttributeProvider member, bool inherit = true) => member.GetAttributes<T>(inherit).FirstOrDefault();

        /// <summary>
        /// 检索应用于类型的成员的自定义属性的数组。参数指定成员、要搜索的自定义属性的类型以及是否搜索成员的祖先。
        /// </summary>
        /// <typeparam name="T"> 要搜索的自定义属性的类型或基类型，仅支持接口和继承于 <see cref="Attribute"/> 的类型。</typeparam>
        /// <param name="member">一个从 <see cref="MemberInfo"/> 类派生的对象，该类描述类的构造函数、事件、字段、方法或属性成员。</param>
        /// <param name="inherit">如果为 true，则指定还在 <paramref name="member"/> 的祖先中搜索自定义属性。</param>
        /// <returns>一个 <typeparamref name="T"/> 数组，包含应用于 <paramref name="member"/> 的 <typeparamref name="T"/> 类型的自定义属性；如果不存在此类自定义属性，则为空数组。</returns>
        public static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider member, bool inherit = true)
        {
            if (member is null) throw new ArgumentNullException(nameof(member));

            var type = typeof(T);
            if (typeof(Attribute).IsAssignableFrom(type))
            {
                foreach (var item in member.GetCustomAttributes(type, inherit))
                {
                    yield return (T)item;
                }
            }
            else
            {
                var typeInfo = type.GetTypeInfo();
                foreach (var item in member.GetCustomAttributes(inherit))
                {
                    if (typeInfo.IsInstanceOfType(item))
                    {
                        yield return (T)item;
                    }
                }
            }
        }

        /// <summary>
        /// 确定是否将任意自定义属性应用于类型成员。参数指定成员、要搜索的自定义属性的类型以及是否搜索成员的祖先。
        /// </summary>
        /// <typeparam name="T">要搜索的自定义属性的类型或基类型。</typeparam>
        /// <param name="member">一个从 <see cref="MemberInfo"/> 类派生的对象，该类描述类的构造函数、事件、字段、方法、类型或属性成员。</param>
        /// <param name="inherit">如果为 true，则指定还在 <paramref name="member"/> 的祖先中搜索自定义属性。</param>
        /// <returns>如果类型 <typeparamref name="T"/> 的某个自定义属性应用于 <paramref name="member"/>，则为 true；否则为 false。</returns>
        public static bool IsDefined<T>(this ICustomAttributeProvider member, bool inherit = true)
        {
            if (member is null) throw new ArgumentNullException(nameof(member));

            var type = typeof(T);

            if (typeof(Attribute).IsAssignableFrom(type))
            {
                return member.IsDefined(type, inherit);
            }

            var typeInfo = type.GetTypeInfo();
            foreach (var item in member.GetCustomAttributes(inherit))
            {
                if (typeInfo.IsInstanceOfType(item)) return true;
            }

            return false;
        }

        public static Type GetControllerReturnType(this MethodInfo method)
        {
            var isAsync = method.IsAsync();
            var returnType = method.ReturnType;
            return isAsync ? (returnType.GenericTypeArguments.FirstOrDefault() ?? typeof(void)) : returnType;
        }
        public static bool IsIn<T>(this T item, params T[] list)
        {
            return list.Contains(item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T As<T>(this object obj)
    where T : class
        {
            return (T)obj;
        }

        /// <summary>
        /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.Type)"/> method.
        /// </summary>
        /// <param name="obj">Object to be converted</param>
        /// <typeparam name="T">Type of the target object</typeparam>
        /// <returns>Converted object</returns>
        public static T To<T>(this object obj)
            where T : struct
        {
            if (typeof(T) == typeof(Guid))
            {
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString());
            }

            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
