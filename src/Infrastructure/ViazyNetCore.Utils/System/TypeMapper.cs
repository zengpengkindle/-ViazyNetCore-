using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 表示一个类型的映射器。
    /// </summary>
    public sealed class TypeMapper
    {
        private static readonly ConcurrentDictionary<Type, TypeMapper> Cacher = new ConcurrentDictionary<Type, TypeMapper>();
        private readonly List<PropertyMapper> _properties;
        private readonly IMapperNameProvider _mapperNameProvider;
        private readonly string _name;

        /// <summary>
        /// 获取实体的属性映射器集合。
        /// </summary>
        public IEnumerable<PropertyMapper> Properties => this._properties;

        /// <summary>
        /// 获取实体的主键属性映射器集合。
        /// </summary>
        public IEnumerable<PropertyMapper> KeyProperties { get; }

        /// <summary>
        /// 获取指定属性名称的属性映射器。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        public PropertyMapper this[string propertyName] => this._properties.Find(this.CreatePredicate(propertyName));

        /// <summary>
        /// 获取或设置映射器的名称。
        /// </summary>
        public string Name => this.GetName(null, null);

        /// <summary>
        /// 获取或设置映射器的显示名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 获取实体的类型。
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 获取实体的属性映射器集合的元素数。
        /// </summary>
        public int Count => this._properties.Count;

        private TypeMapper(Type type)
        {
            this.Type = type;

            if (this._name is null)
            {
                var nameAttr = type.GetAttribute<NameAttribute>();
                this._name = nameAttr?.Name;
            }

            if (this._name is null)
            {
                this._name = type.Name;
            }

            this.DisplayName = type.GetAttribute<ComponentModel.DisplayNameAttribute>()?.DisplayName ?? this._name;

            var ps = type.GetProperties();
            this._properties = new List<PropertyMapper>(ps.Length);
            var keyProperties = new List<PropertyMapper>(1);

            foreach (var p in ps)
            {
                if (p.IsDefined<IgnoreAttribute>() || p.GetIndexParameters().Length > 0) continue;
                var propertyMapper = new PropertyMapper(this, p);
                this._properties.Add(propertyMapper);
                if (propertyMapper.IsKey) keyProperties.Add(propertyMapper);
            }
            this.KeyProperties = keyProperties;

            this._mapperNameProvider = type.GetAttribute<IMapperNameProvider>();
        }

        /// <summary>
        /// 获取指定来源和实体的映射器名称。
        /// </summary>
        /// <param name="source">来源。可以是一个 NULL 值。</param>
        /// <param name="entity">类型映射器关联的对象实例，并将 <paramref name="entity"/> 转换成映射器的类型 。根据不同的来源，可能是一个 null 值。</param>
        /// <returns>映射器名称。</returns>
        public string GetName(string source, object entity)
        {
            if (this._mapperNameProvider is null) return this._name;

            return this._mapperNameProvider.GetName(source, this, entity);
        }

        private Predicate<PropertyMapper> CreatePredicate(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            return p => string.Equals(p.Name, propertyName, StringComparison.CurrentCultureIgnoreCase)
            || string.Equals(p.PropertyName, propertyName, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 指定属性名，判断指定的属性是否存在。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        /// <returns>存在返回 true，否则返回 false。</returns>
        public bool Contains(string propertyName)
        {
            return this._properties.Exists(this.CreatePredicate(propertyName));
        }

        /// <summary>
        /// 检验指定实例所有属性的值。
        /// </summary>
        /// <param name="instance">一个实例，null 值表示检验静态属性。</param>
        public void Validate(object instance)
        {
            foreach (var p in this._properties)
            {
                p.Validate(instance, p.GetValue(instance));
            }
        }

        internal TypeMapper ThrowWithNotFoundKeys()
        {
            if (!this.KeyProperties.Any()) throw new NotSupportedException($"The type '“{this.Type.FullName}”'  does not have an explicit or implicit(Id) primary key.");
            return this;
        }


        /// <summary>
        /// 指定映射的数据类型，创建或从缓存读取一个实体的映射器。
        /// </summary>
        /// <param name="type">映射的数据类型。</param>
        /// <returns>实体映射器。</returns>
        public static TypeMapper Create(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            return Cacher.GetOrAdd(type, t => new TypeMapper(t));
        }
    }

    /// <summary>
    /// 表示一个类型的映射器的静态缓存。
    /// </summary>
    /// <typeparam name="TEntity">映射的数据类型。</typeparam>
    public static class TypeMapper<TEntity>
    {
        /// <summary>
        /// 获取类型的映射器。
        /// </summary>
        public static readonly TypeMapper Instance = TypeMapper.Create(typeof(TEntity));
    }
}
