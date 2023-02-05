using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reflection
{
    /// <summary>
    /// 表示一个属性的映射器。
    /// </summary>
    public sealed class PropertyMapper
    {
        private readonly Lazy<DynamicMemberSetter> _lazySetter;
        private readonly Lazy<DynamicMemberGetter> _lazyGetter;
        private readonly Lazy<object?> _LazyTypeDefaultValue;

        /// <summary>
        /// 获取类型的默认值。
        /// </summary>
        public object? TypeDefaultValue => this._LazyTypeDefaultValue.Value;

        /// <summary>
        /// 获取成员的属性元数据。
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// 获取或设置映射器的名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 获取或设置映射器的显示名称。
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 获取映射器的属性名称。
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// 获取一个值，指示是否为唯一标识。
        /// </summary>
        public bool IsKey { get; }

        /// <summary>
        /// Gets or sets the pattern used to generate values for the property in the database.
        /// </summary>
        public DatabaseGeneratedOption DatabaseGeneratedOption { get; }

        /// <summary>
        /// 获取属性所属的类型映射器。
        /// </summary>
        public TypeMapper TypeMapper { get; }

        /// <summary>
        /// 获取属性验证器数组。
        /// </summary>
        public IEnumerable<IPropertyValidator> Validators { get; }

        /// <summary>
        /// 指定属性元数据，初始化一个 <see cref="PropertyMapper"/> 类的新实例。
        /// </summary>
        /// <param name="typeMapper">类型的映射器。</param>
        /// <param name="property">成员的属性元数据。</param>
        internal PropertyMapper(TypeMapper typeMapper, PropertyInfo property)
        {
            this.Property = property;
            this.TypeMapper = typeMapper;

            this._lazySetter = new Lazy<DynamicMemberSetter>(property.CreatePropertySetter);
            this._lazyGetter = new Lazy<DynamicMemberGetter>(property.CreatePropertyGetter);
            this._LazyTypeDefaultValue = new Lazy<object?>(property.PropertyType.GetDefaultValue);

            this.PropertyName = property.Name;

            var databaseGeneratedAttr = property.GetAttribute<DatabaseGeneratedAttribute>();
            this.DatabaseGeneratedOption = databaseGeneratedAttr is null ? DatabaseGeneratedOption.None : databaseGeneratedAttr.DatabaseGeneratedOption;

            var columnAttr = property.GetAttribute<ColumnAttribute>();
            this.Name = columnAttr?.Name
               ?? property.GetAttribute<NameAttribute>()?.Name
               ?? property.Name;

            this.DisplayName = property.GetAttribute<ComponentModel.DisplayNameAttribute>()?.DisplayName ?? this.Name;

            //var keyAttr = property.GetAttribute<KeyAttribute>();
            //this.IsKey = keyAttr != null || string.Equals(this.Name, Constants.DefaultKeyName, StringComparison.CurrentCultureIgnoreCase);

            this.Validators = property.GetAttributes<IPropertyValidator>().OrderBy(pv => pv.Order);
        }

        /// <summary>
        /// 指定一个实例，设置当前属性的值。
        /// </summary>
        /// <param name="instance">一个实例，null 值表示设置静态属性。</param>
        /// <param name="value">属性的值。</param>
        public void SetValue(object? instance, object? value)
        {
            this._lazySetter.Value(instance, value);
        }

        /// <summary>
        /// 指定一个实例，获取当前属性的值。
        /// </summary>
        /// <param name="instance">一个实例，null 值表示获取静态属性。</param>
        /// <returns>属性的值。</returns>
        public object GetValue(object instance)
        {
            return this._lazyGetter.Value(instance);
        }

        /// <summary>
        /// 检验指定属性的值。
        /// </summary>
        /// <param name="instance">一个实例，null 值表示检验静态属性。</param>
        /// <param name="value">属性的值。</param>
        public void Validate(object instance, object value)
        {
            foreach (var validator in this.Validators)
            {
                validator.Validate(this, instance, value);
            }
            //System.ComponentModel.DataAnnotations.ValidationContext
        }
    }
}