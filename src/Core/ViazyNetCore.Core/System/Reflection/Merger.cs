using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// 定义一个可合并的方法。
    /// </summary>
    /// <typeparam name="TResult">返回的数据类型。</typeparam>
    /// <typeparam name="T">对象的数据类型。</typeparam>
    public interface IMergeable<TResult, T>
    {
        /// <summary>
        /// 忽略给定表达式的属性元素。
        /// </summary>
        /// <param name="ignoreProperty">忽略表达式。</param>
        /// <returns>当前实例。</returns>
        TResult Ignore(Expression<Func<T, object>> ignoreProperty);
        /// <summary>
        /// 当两个合并对象的属性元素发生冲突时，给定使用的属性元素。
        /// </summary>
        /// <param name="useProperty">使用表达式。</param>
        /// <returns>当前实例</returns>
        TResult Use(Expression<Func<T, object>> useProperty);
    }

    /// <summary>
    /// 定义一个合并源的方法。
    /// </summary>
    /// <typeparam name="TSource">源的数据类型。</typeparam>
    public interface IMergeSource<TSource> : IMergeable<IMergeSource<TSource>, TSource>
    {
        /// <summary>
        /// 合并目标对象。
        /// </summary>
        /// <typeparam name="TTarget">目标的数据类型。</typeparam>
        /// <param name="target">目标的对象。</param>
        /// <returns>一个合并目标的方法。</returns>
        IMergeTarget<TTarget> With<TTarget>(TTarget target);
    }

    /// <summary>
    /// 定义一个合并目标的方法。
    /// </summary>
    /// <typeparam name="TTarget">目标的数据类型。</typeparam>
    public interface IMergeTarget<TTarget> : IMergeable<IMergeTarget<TTarget>, TTarget>
    {
        /// <summary>
        /// 生成合并后的新对象。
        /// </summary>
        /// <returns>一个新的对象，包含源和目标的所有属性和值。</returns>
        object Build();
    }

    static class MergerBuilder
    {
        public static readonly AssemblyBuilder AssemblyBuilder;
        public static readonly ModuleBuilder ModuleBuilder;
        const string Name = "AnonymousTypeExentions";
        static MergerBuilder()
        {
            var assembly = new AssemblyName(Name);
            AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assembly, AssemblyBuilderAccess.Run);
            ModuleBuilder = AssemblyBuilder.DefineDynamicModule(Name);
        }

        public static TypeInfo CreateType(string name, PropertyMapper[] pdc)
        {
            //create TypeBuilder
            var typeBuilder = ModuleBuilder.DefineType(name, TypeAttributes.Public, typeof(object));

            //get list of types for ctor definition
            var types = pdc.Select(pd => pd.Property.PropertyType).ToArray();

            //create priate fields for use w/in the ctor body and properties
            var dict = pdc.ToDictionary(pd => pd.Property, pd => typeBuilder.DefineField(string.Format("_{0}", pd.Name), pd.Property.PropertyType, FieldAttributes.Private));

            //define/emit the Ctor
            BuildCtor(typeBuilder, dict.Values.ToArray(), types);

            //define/emit the properties
            BuildProperties(typeBuilder, dict);

            //return Type definition
            return typeBuilder.CreateTypeInfo();
        }

        private static void BuildCtor(TypeBuilder typeBuilder, FieldBuilder[] fields, Type[] types)
        {
            //define ctor()
            ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, types);

            //build ctor()
            ILGenerator ctorGen = ctor.GetILGenerator();

            //create ctor that will assign to private fields
            for (int i = 0; i < fields.Length; i++)
            {
                //load argument (parameter)
                ctorGen.Emit(OpCodes.Ldarg_0);
                ctorGen.Emit(OpCodes.Ldarg, i + 1);

                //store argument in field
                ctorGen.Emit(OpCodes.Stfld, fields[i]);
            }

            //return from ctor()
            ctorGen.Emit(OpCodes.Ret);
        }

        private static void BuildProperties(TypeBuilder typeBuilder, Dictionary<PropertyInfo, FieldBuilder> dict)
        {
            //build properties
            foreach (var item in dict)
            {
                var field = item.Value;

                //remove '_' from name for public property name
                string propertyName = field.Name.Substring(1);

                //define the property
                PropertyBuilder property = typeBuilder.DefineProperty(propertyName,
                    PropertyAttributes.None, field.FieldType, null);
                foreach (var ca in item.Key.GetCustomAttributesData().Select(data =>
                {
                    var ps = data.NamedArguments.Where(d => !d.IsField);
                    var fs = data.NamedArguments.Where(d => d.IsField);
                    return new CustomAttributeBuilder(
                        data.Constructor, data.ConstructorArguments.Select(arg => arg.Value).ToArray()
                        , ps.Select(p => p.MemberInfo).Cast<PropertyInfo>().ToArray(), ps.Select(p => p.TypedValue.Value).ToArray()
                        , fs.Select(p => p.MemberInfo).Cast<FieldInfo>().ToArray(), fs.Select(p => p.TypedValue.Value).ToArray()
                        );
                }))
                {
                    property.SetCustomAttribute(ca);
                }

                //define 'Get' method only (anonymous types are read-only)
                MethodBuilder getMethod = typeBuilder.DefineMethod(
                    string.Format("get_{0}", propertyName),
                    MethodAttributes.Public | MethodAttributes.SpecialName,
                    field.FieldType,
                    Type.EmptyTypes
                    );

                //build 'Get' method
                ILGenerator methGen = getMethod.GetILGenerator();

                //method body
                methGen.Emit(OpCodes.Ldarg_0);
                //load value of corresponding field
                methGen.Emit(OpCodes.Ldfld, field);
                //return from 'Get' method
                methGen.Emit(OpCodes.Ret);

                //assign method to property 'Get'
                property.SetGetMethod(getMethod);
            }
        }

    }
    class MergerData
    {

        private static readonly ConcurrentDictionary<string, TypeInfo> AnonymousTypes = new ConcurrentDictionary<string, TypeInfo>();

        private readonly Lazy<List<string>> _IgnoredProperties = new Lazy<List<string>>();
        public List<string> IgnoredProperties => this._IgnoredProperties.Value;

        private readonly Lazy<List<string>> _UseProperties = new Lazy<List<string>>();
        public List<string> UseProperties => this._UseProperties.Value;
        public object Source { get; private set; }
        public object Target { get; internal set; }

        private MergerData() { }

        public static MergerSource<TSource> From<TSource>(TSource source)
        {
            var data = new MergerData
            {
                Source = source
            };
            return new MergerSource<TSource>(data, source);
        }

        private Tuple<object, PropertyMapper>[] GetProperties()
        {

            var properties = new Dictionary<string, Tuple<object, PropertyMapper>>(StringComparer.OrdinalIgnoreCase);

            var sourceMapper = TypeMapper.Create(this.Source.GetType());
            var targetMapper = TypeMapper.Create(this.Target.GetType());

            bool hasIgnore = this._IgnoredProperties.IsValueCreated;
            bool hasUse = this._UseProperties.IsValueCreated;
            void AddProperties(TypeMapper mapper, object obj, string sourceName, string targetName)
            {
                foreach (var pm in mapper.Properties)
                {
                    if (hasIgnore && this.IgnoredProperties.Contains(this.Concat(sourceName, pm.Name))) continue;
                    if (hasUse && this.UseProperties.Contains(this.Concat(targetName, pm.Name))) continue;

                    properties[pm.Name] = Tuple.Create(obj, pm);
                }
            }

            AddProperties(sourceMapper, this.Source, sourceMapper.Name, targetMapper.Name);
            AddProperties(targetMapper, this.Target, targetMapper.Name, sourceMapper.Name);

            return properties.Values.ToArray();
        }


        public object Build()
        {
            var name = string.Format("{0}_{1}", this.Source.GetType(), this.Target.GetType());
            if (this._IgnoredProperties.IsValueCreated) name += "#IGD_" + string.Join(",", this.IgnoredProperties);
            if (this._UseProperties.IsValueCreated) name += "#USE_" + string.Join(",", this.UseProperties);

            var opms = this.GetProperties();

            var type = AnonymousTypes.GetOrAdd(name, n => MergerBuilder.CreateType(n, opms.Select(t => t.Item2).ToArray()));

            var values = opms.Select(t => t.Item2.GetValue(t.Item1)).ToArray();
            return Activator.CreateInstance(type, values);

        }

        public string Concat(string key1, string key2) => key1 + "." + key2;
    }
    class MergerSource<TSource> : IMergeSource<TSource>, IMergeTarget<TSource>
    {
        public MergerData Data { get; }
        public TSource Source { get; }
        public MergerSource(MergerData data, TSource source)
        {
            this.Data = data ?? throw new ArgumentNullException(nameof(data));
            if (source is null) throw new ArgumentNullException(nameof(source));
            this.Source = source;
        }

        public MergerSource<TSource> Ignore(Expression<Func<TSource, object>> ignoreProperty)
        {
            this.Data.IgnoredProperties.Add(this.GetObjectTypeAndProperty(ignoreProperty));
            return this;
        }

        public MergerSource<TSource> Use(Expression<Func<TSource, object>> useProperty)
        {
            this.Data.UseProperties.Add(this.GetObjectTypeAndProperty(useProperty));
            return this;
        }


        private string GetObjectTypeAndProperty(Expression<Func<TSource, object>> property)
        {
            if (property.Body is MemberExpression)
                return this.Data.Concat(((MemberExpression)property.Body).Member.ReflectedType.UnderlyingSystemType.Name
                    , ((MemberExpression)property.Body).Member.Name);

            if (property.Body is UnaryExpression)
                return this.Data.Concat(((MemberExpression)((UnaryExpression)property.Body).Operand).Member.ReflectedType.UnderlyingSystemType.Name
                    , ((MemberExpression)((UnaryExpression)property.Body).Operand).Member.Name);

            //- 不支持的表达式类型
            throw new NotSupportedException($"Unsupported expression type '“{property.Body.NodeType}”'.");
        }

        public IMergeTarget<TTarget> With<TTarget>(TTarget target)
        {
            this.Data.Target = target;
            return new MergerSource<TTarget>(this.Data, target);
        }

        public object Build() => this.Data.Build();

        IMergeTarget<TSource> IMergeable<IMergeTarget<TSource>, TSource>.Ignore(Expression<Func<TSource, object>> ignoreProperty) => this.Ignore(ignoreProperty);
        IMergeTarget<TSource> IMergeable<IMergeTarget<TSource>, TSource>.Use(Expression<Func<TSource, object>> useProperty) => this.Use(useProperty);
        IMergeSource<TSource> IMergeable<IMergeSource<TSource>, TSource>.Ignore(Expression<Func<TSource, object>> ignoreProperty) => this.Ignore(ignoreProperty);
        IMergeSource<TSource> IMergeable<IMergeSource<TSource>, TSource>.Use(Expression<Func<TSource, object>> useProperty) => this.Use(useProperty);
    }
}
