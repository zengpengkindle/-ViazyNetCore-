namespace ViazyNetCore.Caching
{
    public enum CacheVersionType
    {
        None,
        GlobalVersion,
        AreaVersion
    }

    public enum CachingExpirationType
    {
        /// <summary>
        /// 不变
        /// </summary>
        Invariable,
        /// <summary>
        /// 稳定的
        /// </summary>
        Stable,
        /// <summary>
        /// 相对稳定
        /// </summary>
        RelativelyStable,
        /// <summary>
        /// 常用单一对象
        /// </summary>
        UsualSingleObject,
        /// <summary>
        /// 常用数组对象
        /// </summary>
        UsualObjectCollection,
        /// <summary>
        /// 单一对象
        /// </summary>
        SingleObject,
        /// <summary>
        /// 数组对象
        /// </summary>
        ObjectCollection
    }
    public enum EntityCacheExpirationPolicies
    {
        Normal = 5,
        Stable = 1,
        Usual = 3
    }
}
