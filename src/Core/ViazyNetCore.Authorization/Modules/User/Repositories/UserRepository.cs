using ViazyNetCore.Authorization.Dtos;
using ViazyNetCore.Authorization.Modules;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个用户的服务仓储。
    /// </summary>
    [Injection]
    public class UserRepository : DefaultRepository<BmsUser, string>, IUserRepository
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
        public async Task ModifyPasswordAsync(string password, Guid salt, string id)
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
        public Task RemoveByIdAsync(string id)
        {
            return this.DeleteAsync(u => u.Id == id);
        }

        #endregion

        #region 查询

        /// <summary>
        /// 查询user账号是否存在
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UserExistAsync(string userName, string id)
        {
            return await this.Select.AnyAsync(u => u.Username == userName && u.Id != id);
        }


        ///// <summary>
        ///// 查询所有模型。
        ///// </summary>
        ///// <param name="args">查询参数。</param>
        ///// <returns>模型的集合。</returns>
        //public Task<PageData<UserFindAllModel>> FindAllAsync(string usernameLike, long roleId, ComStatus? status, Pagination args)
        //{
        //    var query = this.Select;
        //    if (usernameLike.IsNotNull()) query = query.Where(u => u.Username.Contains(usernameLike));
        //    if (roleId > 0) query = query.Where(u => u.RoleId == roleId);
        //    if (status.HasValue) query = query.Where(u => u.Status == status.Value);

        //    var query2 = query.From<BmsRole>((u, r) => u.InnerJoin(u => r.Id == u.RoleId)).OrderByDescending((u, r) => u.ModifyTime)
        //                 .WithTempQuery((u, r) => new UserFindAllModel
        //                 {
        //                     Id = u.Id,
        //                     Username = u.Username,
        //                     Nickname = u.Nickname,
        //                     RoleId = u.RoleId,
        //                     RoleName = r.Name,
        //                     Status = u.Status,
        //                     CreateTime = u.CreateTime,
        //                     ModifyTime = u.ModifyTime,
        //                 });

        //    return query2.ToPageAsync(args);
        //}

        /// <summary>
        /// 根据用户名获取UserRoleDTO
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public Task<UserDto> GetUserByUserName(string userName)
        {
            return this.Select.Where(u => u.Username == userName).WithTempQuery(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Password = u.Password,
                PasswordSalt = u.PasswordSalt,
                Nickname = u.Nickname,
                Status = u.Status,
                GoogleKey = u.GoogleKey,
                ExtendData = u.ExtraData,
            }).FirstAsync();

            //return this.Select.From<BmsRole, BmsUserRole>().InnerJoin((u, r, ur) => u.Id == ur.UserId && r.Id == ur.RoleId)
            //    .Where((u, r,ur) => u.Username == userName)
            //    .WithTempQuery((u, r,ur) => new UserRoleDTO
            //    {
            //        Id = u.Id,
            //        Username = u.Username,
            //        Password = u.Password,
            //        PasswordSalt = u.PasswordSalt,
            //        Nickname = u.Nickname,
            //        //RoleId = r.Id,
            //        //RoleName = r.Name,
            //        Status = u.Status,
            //        GoogleKey = u.GoogleKey,
            //        ExtendData = u.ExtraData,
            //        //RoleExtendData = r.ExtraData
            //    }).FirstAsync();
        }

        /// <summary>
        /// 根据用户编号获取可用的用户账号信息
        /// </summary>
        /// <param name="id">用户编号</param>
        /// <returns></returns>
        public Task<BmsUser> GetEnabledUserByIdAsync(string id)
        {
            return this.Select.Where(u => u.Id == id && u.Status == ComStatus.Enabled)
                .WithTempQuery(u => new BmsUser
                {
                    Id = u.Id,
                    Password = u.Password,
                    PasswordSalt = u.PasswordSalt,
                }).FirstAsync();
        }

        #endregion

        #region 谷歌验证码

        /// <summary>
        /// 验证用户是否已绑定谷歌验证码。
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>true：已绑定</returns>
        public async Task<bool> CheckUserBindGoogleAuthenticator(string id)
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
        public Task<bool> BindGoogleAuthenticator(string id, string secretKey)
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
        public Task<bool> ClearGoogleAuthenticator(string id)
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

        public Task<UserFindModel> FindByIdAsync(string id)
        {
            return this.Select.Where(p => p.Id == id).WithTempQuery(p => new UserFindModel
            {
                Id = p.Id,
                ExtraData = p.ExtraData,
                Nickname = p.Nickname,
                Status = p.Status,
                Username = p.Username
            }).FirstAsync();
        }

        public Task<PageData<UserFindAllModel>> FindAllAsync(string usernameLike, string roleId, ComStatus? status, long? orgId, Pagination args)
        {
            var query = this.Select;
            if (usernameLike.IsNotNull()) query = query.Where(u => u.Username.Contains(usernameLike));
            if (status.HasValue) query = query.Where(u => u.Status == status.Value);

            var query2 = query
                .WhereIf(orgId.GetValueOrDefault() != 0, u => this.Orm.Select<BmsUserOrg>().Where(p => p.OrgId == orgId && p.UserId == u.Id).Any())
                .OrderByDescending(u => u.ModifyTime)
                         .WithTempQuery(u => new UserFindAllModel
                         {
                             Id = u.Id,
                             Username = u.Username,
                             Nickname = u.Nickname,
                             Status = u.Status,
                             CreateTime = u.CreateTime,
                             ModifyTime = u.ModifyTime,
                         });

            return query2.ToPageAsync(args);
        }

        //public Task ModifyMerchantPasswordById(string id, string newPassword, Guid salt)
        //{
        //    throw new NotImplementedException();
        //}

        public Task ActivateUsers(IEnumerable<string> userIds, ComStatus status)
        {
            return this.UpdateDiy.Set(p => p.Status == status).Where(p => userIds.Contains(p.Id)).ExecuteAffrowsAsync();
        }

        public Task<IUser> CreateUser(BmsUser user_object, bool ignoreDisallowedUsername)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdByUserName(string username)
        {
            return this.Select.Where(p => p.Username == username).WithTempQuery(p => p.Id).FirstAsync();
        }

        public Task<bool> ResetPassword(BmsUser user, string storedPassword)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
