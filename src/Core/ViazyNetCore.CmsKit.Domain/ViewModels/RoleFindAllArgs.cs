namespace ViazyNetCore.ViewModels
{
    /// <summary>
    /// 表示一个角色的查询参数。
    /// </summary>
    public class RoleFindAllArgs: RoleFindAllEnabledArgs
    {
        /// <summary>
        /// 获取或设置一个值，表示状态，为空表示查询所有。
        /// </summary>
        public ComStatus? Status { get; set; }
    }
}