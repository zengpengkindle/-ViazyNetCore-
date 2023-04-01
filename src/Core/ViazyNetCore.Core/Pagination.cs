using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ViazyNetCore
{

    /// <summary>
    /// 提供分页接口的实现。
    /// </summary>
    public interface IPagination
    {
        /// <summary>
        /// 获取或设置以 1 起始的页码。
        /// </summary>
        int Page { get; set; }
        /// <summary>
        /// 获取或设置分页大小。默认为 10。
        /// </summary>
        int Limit { get; set; }
    }

    /// <summary>
    /// 提供分页接口的实现。
    /// </summary>
    public interface IPagination<TModel> : IPagination
    {
        /// <summary>
        /// 0降序 1升序
        /// </summary>
        int Sort { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        [JsonProperty("sort_field")]
        string SortField { get; set; }
    }

    /// <summary>
    /// 基于页码的分页实现。
    /// </summary>
    public class Pagination : IPagination
    {
        private int _PageNumber = 1;
        /// <summary>
        /// 获取或设置以 1 起始的页码。默认为值 1。
        /// </summary>
        public int Page
        {
            get { return this._PageNumber; }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException(nameof(value));
                this._PageNumber = value;
            }
        }

        private int _PageSize = 10;
        /// <summary>
        /// 获取或设置分页大小。默认为 10。
        /// </summary>
        public int Limit
        {
            get { return this._PageSize; }
            set
            {
                if (value < 1) throw new ArgumentOutOfRangeException(nameof(value));
                this._PageSize = value;
            }
        }

        /// <summary>
        /// 初始化一个 <see cref="Pagination"/> 类的新实例。
        /// </summary>
        public Pagination() { }
    }

    public class Pagination<TModel> : Pagination, IPagination where TModel : class, new()
    {

        /// <summary>
        /// 0降序 1升序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string? SortField { get; set; }
    }



    public class PaginationSort : Pagination, IPagination
    {

        /// <summary>
        /// 0降序 1升序
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string? SortField { get; set; }
    }

    public static class PaginationExtension
    {
        public static string? PaserSort<TModel>(this Pagination<TModel> pagination) where TModel : class, new()
        {

            if (pagination.SortField == null)
                return null;
            var @type = typeof(TModel);
            var propertyInfos = type.GetProperties().Where(p => p.Name.Equals(pagination.SortField, StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (propertyInfos.Count == 0)
                return null;
            if (propertyInfos.Count > 1)
                throw new ArgumentException("sort_field has many values");

            return propertyInfos[0].Name;
        }
    }
}
