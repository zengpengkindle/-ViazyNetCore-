namespace ViazyNetCore
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class PermissionAttribute : Attribute
    {
        public string[] PermissionKeys { get; }
        public PermissionAttribute(params string[] permissionKeys)
        {
            this.PermissionKeys = permissionKeys;
        }
    }

    public static class PermissionIds
    {
        /// <summary>
        /// 用户权限管理
        /// </summary>
        public const string User = "User";

        /// <summary>
        /// 合约管理
        /// </summary>
        public const string Contract = "Contract";

        /// <summary>
        /// 系统配置
        /// </summary>
        public const string Setting = "Setting";

        /// <summary>
        /// 文章管理
        /// </summary>
        public const string Archvie = "Archvie";

        /// <summary>
        /// 匿名
        /// </summary>
        public const string Anonymity = "Anonymity";

        /// <summary>
        /// 会员管理
        /// </summary>
        public const string Member = "Member";

        public const string Product = "Product";

        public const string Stock = "Stock";

        public const string Refund = "Refund";

        public const string Trade = "Trade";
    }
}
