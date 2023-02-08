using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using FreeSql.Internal.Model;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个用户的服务仓储。
    /// </summary>
    [Injection]
    public class UserRepository : DefaultRepository<BmsUser, long>, IUserRepository
    {
        public UserRepository(IFreeSql fsql) : base(fsql)
        {
        }

        #region 新增

        /// <summary>
        /// 根据BmsUser新增
        /// </summary>
        /// <param name="user">BmsUser模型</param>
        /// <param name="passWord">密码</param>
        /// <param name="salt">加密salt</param>
        /// <returns></returns>
        public async Task AddByUserModelAsync(BmsUser user, string passWord, Guid salt)
        {
            await this.InsertAsync(user);
        }

        #endregion

        #region 更新
        /// <summary>
        /// 根据UserModel修改
        /// </summary>
        /// <param name="model">UserModel模型</param>
        /// <returns></returns>
        public async Task ModifyByUserModelAsync(UserModel model)
        {
            await this.UpdateDiy
                    .Where(pg => pg.Id == model.Id)
                    .SetDto(new
                    {
                        model.Username,
                        model.Nickname,
                        model.Status,
                        model.RoleId,
                        ModifyTime = DateTime.Now,
                        model.ExtraData,
                    }).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 根据用户编号修改用户密码。
        /// </summary>
        /// <param name="id">用户编号。</param>
        /// <param name="salt">密码盐。</param>
        /// <param name="password">密码。</param>
        /// <returns>修改成功返回 true 值，否则返回 false 值。</returns>
        public async Task ModifyPasswordAsync(string password, Guid salt, long id)
        {
            await this.UpdateDiy
                    .Where(pg => pg.Id == id)
                    .SetDto(new
                    {
                        Password = password,
                        PasswordSalt = salt,
                        ModifyTime = DateTime.Now,
                    }).ExecuteAffrowsAsync();
        }

        #endregion

        #region 删除
        /// <summary>
        /// 根据id彻底删除模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        public Task RemoveByIdAsync(long id)
        {
            return this.DeleteAsync(u => u.Id == id);
        }

        #endregion

        #region 查询

        /// <summary>
        /// 查询user账号是否存在
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UserExistAsync(string userName, long id)
        {
            return await this.Select.AnyAsync(u => u.Username == userName && u.Id != id);
        }

        /// <summary>
        /// 根据角色编号查询用户表是否有数据。
        /// </summary>
        /// <param name="id">角色编号。</param>
        /// <returns>异步操作。</returns>
        public Task<bool> ExistByIdAsync(long id)
        {
            return this.Select.Where(u => u.RoleId == id).AnyAsync();
        }

        /// <summary>
        /// 查找指定编号的模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>模型。</returns>
        public Task<UserFindModel> FindByIdAsync(long id)
        {
            return this.Select.From<BmsRole>().InnerJoin((u, r) => r.Id == u.RoleId)
                .Where((u, r) => u.Id == id)
                .WithTempQuery((u, r) =>
                    new UserFindModel
                    {
                        Id = u.Id,
                        Username = u.Username,
                        Nickname = u.Nickname,
                        RoleId = u.RoleId,
                        RoleName = r.Name,
                        Status = u.Status,
                        ExtraData = u.ExtraData,
                        GoogleKey = u.GoogleKey
                    }).FirstAsync();
        }

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        public Task<PageData<UserFindAllModel>> FindAllAsync(string usernameLike, long roleId, ComStatus? status, Pagination args)
        {
            var query = this.Select;
            if (usernameLike.IsNotNull()) query = query.Where(u => u.Username.Contains(usernameLike));
            if (roleId > 0) query = query.Where(u => u.RoleId == roleId);
            if (status.HasValue) query = query.Where(u => u.Status == status.Value);

            var query2 = query.From<BmsRole>((u, r) => u.InnerJoin(u => r.Id == u.RoleId)).OrderByDescending((u, r) => u.ModifyTime)
                         .WithTempQuery((u, r) => new UserFindAllModel
                         {
                             Id = u.Id,
                             Username = u.Username,
                             Nickname = u.Nickname,
                             RoleId = u.RoleId,
                             RoleName = r.Name,
                             Status = u.Status,
                             CreateTime = u.CreateTime,
                             ModifyTime = u.ModifyTime,
                         });

            return query2.ToPageAsync(args);
        }

        /// <summary>
        /// 根据用户名获取UserRoleDTO
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public Task<UserRoleDTO> GetUserRoleByUserName(string userName)
        {
            return this.Select.From<BmsRole>().InnerJoin((u, r) => u.RoleId == r.Id)
                .Where((u, r) => u.Username == userName)
                .WithTempQuery((u, r) => new UserRoleDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Password = u.Password,
                    PasswordSalt = u.PasswordSalt,
                    Nickname = u.Nickname,
                    RoleId = u.RoleId,
                    RoleName = r.Name,
                    Status = u.Status,
                    GoogleKey = u.GoogleKey,
                    ExtendData = u.ExtraData,
                    RoleExtendData = r.ExtraData
                }).FirstAsync();
        }

        /// <summary>
        /// 根据用户编号获取可用的用户账号信息
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns></returns>
        public Task<BmsUser> GetEnabledUserByIdAsync(long id)
        {
            return this.Select.Where(u => u.Id == id && u.Status == ComStatus.Enabled)
                .WithTempQuery(u => new BmsUser
                {
                    Id = u.Id,
                    Password = u.Password,
                    PasswordSalt = u.PasswordSalt,
                }).FirstAsync();
        }

        #region 判断

        /// <summary>
        /// 检查默认密码的用户
        /// </summary>
        /// <returns></returns>
        public async Task<List<BmsUser>> CheckDefaultPassword()
        {
            List<BmsUser> merchants = new List<BmsUser>();

            var list = await this.Select.ToListAsync();

            foreach (var user in list)
            {
                if (user.Password == DataSecurity.GenerateSaltedHash(Globals.DefaultPassword, user.PasswordSalt))
                {
                    merchants.Add(user);
                }
            }
            return merchants;
        }


        #endregion

        #endregion

        #region 谷歌验证码

        /// <summary>
        /// 验证用户是否已绑定谷歌验证码。
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>true：已绑定</returns>
        public async Task<bool> CheckUserBindGoogleAuthenticator(long id)
        {
            var result = await (this.Select.Where(u => u.Id == id).WithTempQuery(u =>
                               new { u.Id, u.GoogleKey }).FirstAsync());
            if (result == null) throw new ApiException("user is invalid.");
            return result.GoogleKey.IsNotNull();
        }

        /// <summary>
        /// 绑定谷歌验证码。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public Task<bool> BindGoogleAuthenticator(long id, string secretKey)
        {
            return this.UpdateDiy
                     .Where(pg => pg.Id == id)
                     .SetDto(new
                     {
                         GoogleKey = secretKey,
                         ModifyTime = DateTime.Now,
                     }).ExecuteAffrowsAsync()
                     .ContinueWith(t => t.Result == 1);
        }


        /// <summary>
        /// 重置谷歌验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> ClearGoogleAuthenticator(long id)
        {
            return this.UpdateDiy
                     .Where(pg => pg.Id == id)
                     .SetDto(new
                     {
                         GoogleKey = string.Empty,
                         ModifyTime = DateTime.Now,
                     }).ExecuteAffrowsAsync()
                     .ContinueWith(t => t.Result == 1);
        }

        public Task ModifyMerchantPasswordById(long id, string newPassword, Guid salt)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
