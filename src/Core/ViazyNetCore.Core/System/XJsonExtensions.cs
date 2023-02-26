namespace System
{
    /// <summary>
    /// 表示一个 JSON 的扩展方法。
    /// </summary>
    public static class XJsonExtensions
    {
        /// <summary>
        /// 将当前时间转换为 Javascript Time 值。同等于 JS 代码：“(new Date()).getTime()”
        /// </summary>
        /// <param name="dateTime">时间。</param>
        /// <returns>一个 Javascript Time 值。</returns>
        public static long ToJsTime(this DateTime dateTime) => (dateTime.ToUniversalTime().Ticks - 621355968000000000L) / 10000L;

        /// <summary>
        /// 将当前 Javascript Time 值转换为时间。
        /// </summary>
        /// <param name="time">一个 Javascript Time 值</param>
        /// <returns>时间。</returns>
        public static DateTime FromJsTime(this long time) => new DateTime(time * 10000 + 621355968000000000L, DateTimeKind.Utc).ToLocalTime();
    }
}
