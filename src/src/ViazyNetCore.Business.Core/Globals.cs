namespace ViazyNetCore
{
    public static class Globals
    {
        /// <summary>
        /// 管理员角色Id
        /// </summary>
        public const long ADMIN_ROLE_ID=0;
#pragma warning disable IDE1006 // 命名样式
        public const string DefaultPassword = "@dmin!";
        public static string DefaultRandomPassword = "p@sSwd!";

        public static string GetRandomPassword()
        {
            return FastRandom.Instance.NextString(8);
        }
#pragma warning restore IDE1006 // 命名样式
    }
}
