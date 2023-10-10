using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Identity.Domain;

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
        public async Task ModifyByUserModelAsync(UserBaseDto model)
        {
            await this.UpdateDiy
                    .Where(pg => pg.Id == model.Id)
                    .SetDto(new
                    {
                        model.Username,
                        model.Nickname,
                        model.Status,
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
        public async Task<bool> ModifyPasswordAsync(string password, Guid salt, long id)
        {
            var result = await this.UpdateDiy
                       .Where(u => u.Id == id)
                       .Set(p => p.Password == password)
                       .Set(p => p.PasswordSalt == salt)
                       .Set(p => p.ModifyTime == DateTime.Now)
                       .ExecuteAffrowsAsync();
            return result == 1;
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
        /// 根据用户名获取UserRoleDTO
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public Task<BmsUser> GetUserByUserName(string userName)
        {
            return this.Select.Where(u => u.Username == userName).FirstAsync();
        }

        /// <summary>
        /// 根据用户编号获取可用的用户账号信息
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns></returns>
        public Task<BmsUser> GetEnabledUserByIdAsync(long id)
        {
            return this.Select.Where(u => u.Id == id && u.Status == ComStatus.Enabled)
                .FirstAsync();
        }

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

        public Task<UserWithGoogleKeyDto> FindByIdAsync(long id)
        {
            return this.Select.Where(p => p.Id == id).WithTempQuery(p => new UserWithGoogleKeyDto
            {
                Id = p.Id,
                ExtraData = p.ExtraData,
                Nickname = p.Nickname,
                Status = p.Status,
                Username = p.Username,
                PhoneNumber = p.PhoneNumber,
                GoogleKey = p.GoogleKey,
            }).FirstAsync();
        }

        public Task<PageData<UserDto>> FindAllAsync(string usernameLike, string roleId, ComStatus? status, long? orgId, Pagination args)
        {
            var query = this.Select;
            if (usernameLike.IsNotNull()) query = query.Where(u => u.Username.Contains(usernameLike));
            if (status.HasValue) query = query.Where(u => u.Status == status.Value);

            var query2 = query
                .WhereIf(orgId.GetValueOrDefault() != 0, u => this.Orm.Select<BmsUserOrg>().Where(p => p.OrgId == orgId && p.UserId == u.Id).Any())
                .OrderByDescending(u => u.ModifyTime)
                         .WithTempQuery(u => new UserDto
                         {
                             Id = u.Id,
                             Username = u.Username,
                             PhoneNumber = u.PhoneNumber,
                             Nickname = u.Nickname,
                             Status = u.Status,
                             CreateTime = u.CreateTime,
                             ModifyTime = u.ModifyTime,
                             ExtraData = u.ExtraData,
                             OrgId = u.Id,
                         });

            return query2.ToPageAsync(args);
        }

        //public Task ModifyMerchantPasswordById(string id, string newPassword, Guid salt)
        //{
        //    throw new NotImplementedException();
        //}

        public Task ActivateUsers(IEnumerable<long> userIds, ComStatus status)
        {
            return this.UpdateDiy.Set(p => p.Status == status).Where(p => userIds.Contains(p.Id)).ExecuteAffrowsAsync();
        }

        public Task<IUser> CreateUser(BmsUser user_object, bool ignoreDisallowedUsername)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetUserIdByUserName(string username)
        {
            return this.Select.Where(p => p.Username == username).WithTempQuery(p => p.Id).FirstAsync();
        }

        public Task<int> UpdateUserInfoAsync(long id, UserUpdateDto updateDto)
        {
            return this.UpdateDiy
                .Set(p => p.Nickname == updateDto.NickName)
                .Set(p => p.ExtraData == updateDto.ExtraData)
                  .Where(p => p.Id == id)
                  .ExecuteAffrowsAsync();
        }

        public Task<int> UpdateUserAvatarAsync(long id, UserAvatarDto updateDto)
        {
            return this.UpdateDiy
                .Set(p => p.Avatar == updateDto.Avatar)
                  .Where(p => p.Id == id)
                  .ExecuteAffrowsAsync();
        }

        public async Task EnsureCollectionLoadedAsync<TProperty>(BmsUser user
            , Expression<Func<BmsUser, TProperty>> propertyExpression
            , CancellationToken cancellationToken) where TProperty : class
        {
            var bms = await this.Select.Where(p => p.Id == user.Id)
                .Include(propertyExpression)
                .FirstAsync();
            user.Claims.AddRange(bms.Claims);
        }
        #endregion

    }
}
