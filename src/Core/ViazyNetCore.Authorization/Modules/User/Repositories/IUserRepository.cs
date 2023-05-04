using ViazyNetCore.Authorization.Dtos;
using ViazyNetCore.Authorization.Modules;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个用户的服务仓储接口。
    /// </summary>
    [Injection]
    public interface IUserRepository : IBaseRepository<BmsUser, long>
    {
        /// <summary>
        /// 查询user账号是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> UserExistAsync(string userName, long id);

        /// <summary>
        /// 根据BmsUser新增
        /// </summary>
        /// <param name="user">BmsUser模型</param>
        /// <param name="passWord">密码</param>
        /// <param name="salt">加密salt</param>
        /// <returns></returns>
        Task AddByUserModelAsync(BmsUser user, string passWord, Guid salt);

        /// <summary>
        /// 根据UserModel修改
        /// </summary>
        /// <param name="model">UserModel模型</param>
        /// <returns></returns>
        Task ModifyByUserModelAsync(UserModel model);

        /// <summary>
        /// 根据id彻底删除模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        Task RemoveByIdAsync(long id);

        /// <summary>
        /// 查找指定编号的模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>模型。</returns>
        Task<UserFindModel> FindByIdAsync(long id);

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        Task<PageData<UserFindAllModel>> FindAllAsync(string usernameLike, string roleId, ComStatus? status, long? orgId, Pagination args);

        /// <summary>
        /// 根据用户名获取UserRoleDTO
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        Task<BmsUser> GetUserByUserName(string userName);

        /// <summary>
        /// 重置指定用户编号的密码。
        /// </summary>
        /// <param name="id">用户编号。</param>
        /// <param name="salt">密码盐。</param>
        /// <param name="password">密码。</param>
        /// <returns>重置成功返回 true 值，否则返回 false 值。</returns>
        Task ModifyPasswordAsync(string password, Guid salt, long id);

        /// <summary>
        /// 根据用户编号获取可用的用户账号信息
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns></returns>
        Task<BmsUser> GetEnabledUserByIdAsync(long id);

        ///// <summary>
        ///// 根据编号修改商户密码
        ///// </summary>
        ///// <param name="id">商户编号</param>
        ///// <param name="newPassword">新密码</param>
        ///// <param name="salt">密码盐</param>
        ///// <returns></returns>
        //Task ModifyMerchantPasswordById(string id, string newPassword, Guid salt);

        /// <summary>
        /// 验证用户是否已绑定谷歌验证码。
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>true：已绑定</returns>
        Task<bool> CheckUserBindGoogleAuthenticator(long id);

        /// <summary>
        /// 绑定谷歌验证码。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        Task<bool> BindGoogleAuthenticator(long id, string secretKey);

        /// <summary>
        /// 重置谷歌验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ClearGoogleAuthenticator(long id);

        #region 判断
        Task ActivateUsers(IEnumerable<long> userIds, ComStatus status);
        Task<IUser> CreateUser(BmsUser user_object, bool ignoreDisallowedUsername);
        Task<long> GetUserIdByUserName(string takeOverUserName);
        Task<bool> ResetPassword(BmsUser user, string storedPassword);

        #endregion

    }
}
