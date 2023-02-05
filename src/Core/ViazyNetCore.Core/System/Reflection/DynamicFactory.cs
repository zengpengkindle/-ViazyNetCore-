using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Reflection
{
    /// <summary>
    /// 表示一个动态成员反射的工厂。
    /// </summary>
    public static class DynamicFactory
    {
        private static readonly MethodInfo ChangeTypeMethod = typeof(XTypeExtensions).GetMethod(nameof(XTypeExtensions.Parse)
            , new Type[] { typeof(Type), typeof(object) });
        private static readonly ConcurrentDictionary<MemberInfo, DynamicMemberGetter> GetterCache = new ConcurrentDictionary<MemberInfo, DynamicMemberGetter>();
        private static readonly ConcurrentDictionary<MemberInfo, DynamicMemberSetter> SetterCache = new ConcurrentDictionary<MemberInfo, DynamicMemberSetter>();
        private static readonly ConcurrentDictionary<MethodInfo, DynamicMethodInvoker> MethodCache = new ConcurrentDictionary<MethodInfo, DynamicMethodInvoker>();
        private static readonly ConcurrentDictionary<ConstructorInfo, DynamicConstructorHandler> ConstructorCache = new ConcurrentDictionary<ConstructorInfo, DynamicConstructorHandler>();

        private static Expression CastTo(Expression expression, Type type)
           => Expression.Convert(Expression.Call(ChangeTypeMethod, Expression.Constant(type), expression), type);

        private static Expression CreateMethodCallBody(MethodBase m, ParameterExpression parameters, Type returnType, Func<IEnumerable<ParameterExpression>, Expression> callFunc)
        {
            var parameterInfos = m.GetParameters();  //- 方法的参数集合
            var parameterLength = parameterInfos.Length;

            var variables = new List<ParameterExpression>(parameterLength);
            var expressions = new List<Expression>(parameterLength + 5);
            List<Expression> afterCallExpressions = null;

            for (int i = 0; i < parameterLength; i++)
            {
                var parameterInfo = parameterInfos[i];
                var parameterType = parameterInfo.ParameterType;
                var isByRef = parameterType.IsByRef;
                if (isByRef) parameterType = parameterType.GetElementType();
                var parameterValue = CastTo(Expression.ArrayIndex(parameters, Expression.Constant(i)), parameterType);
                var variable = Expression.Variable(parameterType, parameterInfo.Name);

                expressions.Add(Expression.Assign(variable, parameterValue));
                variables.Add(variable);
                if (isByRef)
                {
                    if (afterCallExpressions is null) afterCallExpressions = new List<Expression>(1);
                    var setArrayValueExpression = Expression.Assign(Expression.ArrayAccess(parameters, Expression.Constant(i)), Expression.Convert(variable, typeof(object)));

                    afterCallExpressions.Add(setArrayValueExpression);
                }
            }
            var callBody = callFunc(variables);

            var resultVariable = Expression.Variable(typeof(object), "result");
            if (returnType == typeof(void))
            {
                expressions.Add(callBody);
                expressions.Add(Expression.Assign(resultVariable, Expression.Constant(null, typeof(object))));
            }
            else
            {
                expressions.Add(Expression.Assign(resultVariable, Expression.Convert(callBody, typeof(object))));
            }

            if (afterCallExpressions != null) expressions.AddRange(afterCallExpressions);

            expressions.Add(resultVariable);
            var body = Expression.Block(variables.InsertAfter(resultVariable), expressions);
            return body;
        }

        private static Expression CreateInstanceCast(bool isStatic, ParameterExpression instance, Type declaringType)
        {
            if (isStatic) return null;
            if (declaringType.IsValueType) return Expression.Unbox(instance, declaringType);
            return Expression.Convert(instance, declaringType);
        }

        /// <summary>
        /// 创建字段的获取器委托。
        /// </summary>
        /// <param name="fieldInfo">字段元数据。</param>
        /// <returns>字段获取器的委托。</returns>
        public static DynamicMemberGetter CreateFieldGetter(this FieldInfo fieldInfo)
        {
            if (fieldInfo is null) throw new ArgumentNullException(nameof(fieldInfo));

            return GetterCache.GetOrAdd(fieldInfo, m =>
            {
                var instance = Expression.Parameter(typeof(object), "instance");
                var instanceCast = CreateInstanceCast(fieldInfo.IsStatic, instance, fieldInfo.DeclaringType);

                var body = Expression.Convert(Expression.Field(instanceCast, fieldInfo), typeof(object));
                return Expression.Lambda<DynamicMemberGetter>(body, instance).Compile();
            });
        }

        /// <summary>
        /// 创建字段的设置器委托。
        /// </summary>
        /// <param name="fieldInfo">字段元数据。</param>
        /// <returns>字段获取器的委托。</returns>
        public static DynamicMemberSetter CreateFieldSetter(this FieldInfo fieldInfo)
        {
            if (fieldInfo is null) throw new ArgumentNullException(nameof(fieldInfo));

            return SetterCache.GetOrAdd(fieldInfo, m =>
            {
                var instance = Expression.Parameter(typeof(object), "instance");
                var value = Expression.Parameter(typeof(object), "value");

                var instanceCast = CreateInstanceCast(fieldInfo.IsStatic, instance, fieldInfo.DeclaringType);
                var valueCast = CastTo(value, fieldInfo.FieldType);

                var body = Expression.Assign(Expression.Field(instanceCast, fieldInfo), valueCast);
                return Expression.Lambda<DynamicMemberSetter>(body, instance, value).Compile();
            });
        }

        /// <summary>
        /// 创建属性的设置器委托。
        /// </summary>
        /// <param name="propertyInfo">属性元数据。</param>
        /// <returns>属性获取器的委托。</returns>
        public static DynamicMemberSetter CreatePropertySetter(this PropertyInfo propertyInfo)
        {
            if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));

            return SetterCache.GetOrAdd(propertyInfo, m =>
            {
                var declaringType = propertyInfo.DeclaringType;
                var methodInfo = propertyInfo.GetSetMethod(true) ?? throw new MissingMemberException(declaringType.FullName, propertyInfo.Name);

                var instance = Expression.Parameter(typeof(object), "instance");
                var value = Expression.Parameter(typeof(object), "value");

                var instanceCast = CreateInstanceCast(methodInfo.IsStatic, instance, declaringType);
                var valueCast = CastTo(value, propertyInfo.PropertyType);

                var body = Expression.Call(instanceCast, methodInfo, valueCast);
                return Expression.Lambda<DynamicMemberSetter>(body, instance, value).Compile();
            });
        }

        /// <summary>
        /// 创建属性的获取器委托。
        /// </summary>
        /// <param name="propertyInfo">属性元数据。</param>
        /// <returns>属性获取器的委托。</returns>
        public static DynamicMemberGetter CreatePropertyGetter(this PropertyInfo propertyInfo)
        {
            if (propertyInfo is null) throw new ArgumentNullException(nameof(propertyInfo));

            return GetterCache.GetOrAdd(propertyInfo, m =>
            {
                var declaringType = propertyInfo.DeclaringType;
                var methodInfo = propertyInfo.GetGetMethod(true) ?? throw new MissingMemberException(declaringType.FullName, propertyInfo.Name);

                var instance = Expression.Parameter(typeof(object), "instance");

                var instanceCast = CreateInstanceCast(methodInfo.IsStatic, instance, declaringType);
                var body = Expression.Convert(Expression.Call(instanceCast, methodInfo), typeof(object));

                return Expression.Lambda<DynamicMemberGetter>(body, instance).Compile();
            });
        }

        /// <summary>
        /// 创建方法的调用委托。
        /// </summary>
        /// <param name="methodInfo">方法元数据。方法不能是一个尚未构造泛型参数的方法</param>
        /// <returns>方法调用的委托。</returns>
        public static DynamicMethodInvoker CreateMethodInvoker(this MethodInfo methodInfo)
        {
            if (methodInfo is null) throw new ArgumentNullException(nameof(methodInfo));
            //- 不支持尚未构造泛型参数的方法。
            if (methodInfo.IsGenericMethodDefinition) throw new ArgumentException("Methods that have not yet constructed generic parameters are not supported.", nameof(methodInfo));

            return MethodCache.GetOrAdd(methodInfo, m =>
            {
                var instance = Expression.Parameter(typeof(object), "instance");
                var parameters = Expression.Parameter(typeof(object[]), "parameters");

                var instanceCast = CreateInstanceCast(m.IsStatic, instance, m.DeclaringType);
                var body = CreateMethodCallBody(m, parameters, m.ReturnType, variables => Expression.Call(instanceCast, methodInfo, variables));

                return Expression.Lambda<DynamicMethodInvoker>(body, instance, parameters).Compile();
            });
        }

        /// <summary>
        /// 创建指定 <paramref name="constructorInfo"/> 的动态构造函数。
        /// </summary>
        /// <param name="constructorInfo">构造函数的元数据。</param>
        /// <returns>绑定到实例构造函数的委托。</returns>
        public static DynamicConstructorHandler CreateConstructorHandler(this ConstructorInfo constructorInfo)
        {
            if (constructorInfo is null) throw new ArgumentNullException(nameof(constructorInfo));

            return ConstructorCache.GetOrAdd(constructorInfo, m =>
            {
                var parameters = Expression.Parameter(typeof(object[]), "parameters");

                var body = CreateMethodCallBody(m, parameters, m.DeclaringType, variables => Expression.New(m, variables));

                return Expression.Lambda<DynamicConstructorHandler>(body, parameters).Compile();
            });
        }

        /// <summary>
        /// 创建指定 <paramref name="type"/> 的动态构造函数。
        /// </summary>
        /// <param name="type">构造函数的定义类。</param>
        /// <param name="types">表示需要的构造函数的参数个数、顺序和类型的 <see cref="Type"/> 对象的数组。- 或 -<see cref="Type"/> 对象的空数组，用于获取不带参数的构造函数。这样的空数组由 static 字段 <see cref="Type.EmptyTypes"/> 提供。</param>
        /// <returns>绑定到实例构造函数的委托。</returns>
        public static DynamicConstructorHandler CreateConstructorHandler(this Type type, params Type[] types)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, types, null);
            if (ctor is null)
            {
                //-值类型，无构造函数
                if (type.IsValueType) return args => Activator.CreateInstance(type, true);
                throw new MissingMemberException(type.FullName, types.Join(t => t.Name, start: "ctor(", end: ")"));
            }
            return CreateConstructorHandler(ctor);
        }
    }
}