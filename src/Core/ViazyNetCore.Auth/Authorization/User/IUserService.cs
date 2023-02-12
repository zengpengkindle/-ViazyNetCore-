using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViazyNetCore.Authorization.Models;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Dtos;

namespace ViazyNetCore.Modules
{
    [Injection]
    public interface IUserService
    {
        /// <summary>
        /// 添加或修改模型。
        /// </summary>
        /// <param name="model">模型。</param>
        /// <param name="randPwd">随机生成的密码,只有新增的时候用到</param>
        /// <returns>模型的编号。</returns>
        Task<string> ManageAsync(UserModel model, string randPwd);

        /// <summary>
        /// 彻底删除模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        Task RemoveAsync(string id);

        /// <summary>
        /// 查找指定编号的模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>模型。</returns>
        Task<UserFindModel> FindAsync(string id);

        /// <summary>
        /// 查询修改时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DateTime> FindModifyTimeAsync(string id);

        /// <summary>
        /// 查找指定编号用户名
        /// </summary>
        /// <param name="id">模型编号</param>
        /// <returns></returns>
        Task<string> GetUsername(string id);

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        Task<PageData<UserFindAllModel>> FindAllAsync(UserFindAllArgs args);

        /// <summary>
        /// 获取登录标识。
        /// </summary>
        /// <param name="args">登录模型参数。</param>
        /// <returns>登录标识。</returns>
        Task<BmsIdentity> GetUserLoginIdentityAsync(UserLoginArgs args, string ip, bool enableGoogleToken);

        /// <summary>
        /// 重置指定用户编号的密码。
        /// </summary>
        /// <param name="id">用户编号。</param>
        /// <returns>重置成功返回 true 值，否则返回 false 值。</returns>
        Task<string> ResetPasswordAsync(string id);

        /// <summary>
        /// 修改指定用户的密码。
        /// </summary>
        /// <param name="id">用户编号。</param>
        /// <param name="args">参数。</param>
        /// <returns>修改成功返回 true 值，否则返回 false 值。</returns>
        Task<bool> ModifyPasswordAsync(string id, UserModifyPasswordArgs args);


        /// <summary>
        /// 多次重复登录验证
        /// </summary>
        /// <param name="username">用户账号</param>
        /// <returns></returns>
        UserLoginCheck GetByUsernameCache(string username, bool state, string ip = null, string? userId = null);

        /// <summary>
        /// 清除账号登录缓存
        /// </summary>
        /// <param name="username">登录账号</param>
        void ClearCache(string username);

        /// <summary>
        /// 验证用户是否已绑定谷歌验证码。
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>true：已绑定</returns>
        Task<bool> CheckUserBindGoogleAuthenticator(string id);

        /// <summary>
        /// 绑定谷歌验证码。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        Task<bool> BindGoogleAuthenticator(string id, string secretKey);

        /// <summary>
        /// 重置谷歌验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ClearGoogleAuthenticator(string id);

        /// <summary>
        /// 校验谷歌验证码
        /// </summary>
        /// <param name="googleKey">商户谷歌验证码</param>
        /// <param name="code">校验码</param>
        /// <param name="enableGoogleToken">是否开启谷歌验证</param>
        /// <returns></returns>
        bool CheckGoogleKey(string googleKey, string code, bool enableGoogleToken);
        
        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        Task<List<BmsUser>> ListAsync();

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        Task<bool> AnyAsync();
        Task<IUser<string>> GetUser(string userId);
    }

}
