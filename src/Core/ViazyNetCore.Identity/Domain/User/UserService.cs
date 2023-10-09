using System.Threading.Tasks;
using System.Linq;
using ViazyNetCore.Caching;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Auth;
using Microsoft.Extensions.Options;
using ViazyNetCore.Authorization.Repositories;
using ViazyNetCore.Identity;
using ViazyNetCore.Identity.Domain;
using ViazyNetCore.Data.FreeSql.Extensions;

namespace ViazyNetCore.Modules
{
    /// <summary>
    /// 表示一个用户的服务。
    /// </summary>
    [Injection]
    public class UserService : IUserService
    {
        public const int LOGIN_MAXCOUNT = 5;
        public const double LOGIN_TIME = 30;
        private readonly IUserRepository _userRepository;
        //private readonly IEventBus _eventBus;
        private readonly ICacheService _cacheService;
        private readonly IUserOrgRepository _userOrgRepository;
        private readonly UserOption _userOption;

        public UserService(IUserRepository userRepository
            , ICacheService cacheService, IOptions<UserOption> options
            , IUserOrgRepository userOrgRepository)
        {
            this._cacheService = cacheService;
            this._userOrgRepository = userOrgRepository;
            //this._eventBus = eventBus;
            this._userRepository = userRepository;
            this._userOption = options.Value;

        }
        /// <summary>
        /// 添加或修改模型。
        /// </summary>
        /// <param name="model">模型。</param>
        /// <param name="randPwd">随机生成的密码,只有新增的时候用到</param>
        /// <returns>模型的编号。</returns>
        public async Task<long> ManageAsync(UserBaseDto model, string randPwd)
        {
            if (await _userRepository.UserExistAsync(model.Username, model.Id))
                throw new ApiException("用户账号已存在。");

            if (model.Id == 0)
            {
                var password = DataSecurity.GenerateSaltedHash(randPwd.ToMd5(), out var salt);
                var user = new BmsUser
                {
                    Username = model.Username,
                    Password = password,
                    PasswordSalt = salt,
                    Nickname = model.Nickname,
                    Status = model.Status,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                    ExtraData = model.ExtraData,
                };
                await _userRepository.AddByUserModelAsync(user, password, salt);
                model.Id = user.Id;
            }
            else
            {
                await _userRepository.ModifyByUserModelAsync(model);
                await this._userOrgRepository.DeleteAsync(a => a.UserId == model.Id);

            }

            if (model.OrgIds != null && model.OrgIds.Any())
            {
                var orgs = model.OrgIds.Select(orgId => new BmsUserOrg
                {
                    UserId = model.Id,
                    OrgId = orgId,
                    IsManager = orgId == model.OrgId,
                    CreateTime = DateTime.Now,
                    ModifyTime = DateTime.Now,
                }).ToList();
                await _userOrgRepository.InsertAsync(orgs);
            }
            return model.Id;
        }

        /// <summary>
        /// 彻底删除模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>异步操作。</returns>
        public async Task RemoveAsync(long id)
        {
            var cacheKey = this.GetCacheKey_GetUser(id);
            this._cacheService.Remove(cacheKey);
            await _userRepository.RemoveByIdAsync(id);
            this._cacheService.Remove(cacheKey);
        }

        /// <summary>
        /// 查找指定编号的模型。
        /// </summary>
        /// <param name="id">模型的编号。</param>
        /// <returns>模型。</returns>
        public async Task<UserWithGoogleKeyDto> FindAsync(long id)
        {
            var result = await _userRepository.FindByIdAsync(id);

            var userOrgs = await _userOrgRepository.Where(p => p.UserId == id).ToListAsync();
            result.OrgIds = userOrgs.Select(p => p.OrgId).ToList();
            result.OrgId = userOrgs.Where(p => p.IsManager).Select(p => p.OrgId).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 查询用户的修改时间
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<DateTime> FindModifyTimeAsync(long id)
        {
            return await this._userRepository.Where(o => o.Id == id).FirstAsync(o => o.ModifyTime);
        }

        public Task<string> GetUsername(long id)
        {
            return _userRepository.Where(p => p.Id == id).WithTempQuery(p => p.Username).FirstAsync();
        }

        public async Task<BmsIdentity> GetUserIdentity(long id)
        {
            var user = await this.GetUser(id) as BmsUser;
            return new BmsIdentity
            {
                Id = user.Id,
                Username = user.Username,
                Nickname = user.Nickname,
                Avatar = user.Avatar
            };
        }

        /// <summary>
        /// 查询所有模型。
        /// </summary>
        /// <param name="args">查询参数。</param>
        /// <returns>模型的集合。</returns>
        public Task<PageData<UserDto>> FindAllAsync(UserFindQueryDto args)
        {
            return _userRepository.FindAllAsync(args.UsernameLike, args.RoleId, args.Status, args.OrgId, args);
        }

        /// <summary>
        /// 获取bms登录标识。
        /// </summary>
        /// <param name="args">登录模型参数。</param>
        /// <returns>登录标识。</returns>
        public async Task<BmsIdentity> GetUserLoginIdentityAsync(UserLoginInputDto args, string ip, bool enableGoogleToken)
        {
            var user = await _userRepository.GetUserByUserName(args.Username);
            if (user != null && user.Status != ComStatus.Deleted)
            {
                args.Auditor = user.Id;
                if (user.Status != ComStatus.Enabled) throw new ApiException("账号已被禁用!");
                //谷歌校验码
                var res = this.CheckGoogleKey(user.GoogleKey, args.Code, this._userOption.EnableGoogleToken);
                if (!res)
                {
                    this.GetByUsernameCache(args.Username, false);
                    throw new ApiException("验证码过期或已失效.");
                }

                //管理员 授权所有按钮权限
                //if (user.RoleId == Globals.ADMIN_ROLE_ID) permissions = new List<string>() { ((int)BMSPermissionCode.All).ToString() };

                if (UserPasswordHelper.CheckPassword(args.Password, user.Password, user.PasswordSalt, this._userOption.UserPasswordFormat))
                {
                    this.GetByUsernameCache(args.Username, true);
                    return new BmsIdentity
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Nickname = user.Nickname,
                        Avatar = user.Avatar,
                        //RoleId = user.RoleId,
                        //RoleName = user.RoleName ?? "超级管理员",
                        BindGoogleAuth = !enableGoogleToken || user.GoogleKey.IsNotNull(),
                        //Permissions = permissions
                    };
                }
            }

            var userLoginCheck = this.GetByUsernameCache(args.Username, false, ip, args.Auditor);
            if (LOGIN_MAXCOUNT - userLoginCheck.ErrorCount == 0)
                throw new ApiException($"您的账号已被锁定，请在 {LOGIN_TIME} 分钟后重试");
            else
                throw new ApiException(string.Format("您的账号或密码错误，您还可以尝试 {0} 次", LOGIN_MAXCOUNT - userLoginCheck.ErrorCount));
        }

        /// <summary>
        /// 重置指定用户编号的密码。
        /// </summary>
        /// <param name="id">用户编号。</param>
        /// <returns>重置成功返回 随机密码,否则抛出异常</returns>
        public async Task<string> ResetPasswordAsync(long id, string randPwd)
        {
            var salt = Guid.NewGuid();
            var password = UserPasswordHelper.EncodePassword(randPwd, salt, this._userOption.UserPasswordFormat);
            await _userRepository.ModifyPasswordAsync(password, salt, id);
            return randPwd;
        }


        /// <summary>
        /// 修改指定用户的密码。
        /// </summary>
        /// <param name="id">用户编号。</param>
        /// <param name="args">参数。</param>
        /// <returns>修改成功返回 true 值，否则返回 false 值。</returns>
        public async Task<bool> ModifyPasswordAsync(long id, UserModifyPasswordEditDto args)
        {
            if (args.NewPassword.Length < 6)
                throw new ApiException("密码长度不能小于6位");

            var user = await _userRepository.GetEnabledUserByIdAsync(id);

            if (user == null || !UserPasswordHelper.CheckPassword(args.OldPassword, user.Password, user.PasswordSalt, this._userOption.UserPasswordFormat))
            {
                return false;
            }

            var salt = Guid.NewGuid();
            var password = UserPasswordHelper.EncodePassword(args.NewPassword, salt, this._userOption.UserPasswordFormat);
            return await _userRepository.ModifyPasswordAsync(password, salt, id);
        }

        #region 密码缓存

        /// <summary>
        /// 多次重复登录验证
        /// </summary>
        /// <param name="username">用户账号</param>
        /// <returns></returns>
        public UserLoginCheckDto GetByUsernameCache(string username, bool state, string ip = null, long? userId = null)
        {
            var cacheKey = GetUsernameCacheKey(username);
            var result = this._cacheService.GetFromFirstLevel<UserLoginCheckDto>(cacheKey);

            //此账号此时还没有缓存
            if (result == null)
            {
                //正确直接返回
                if (state == true) { return result; };
                //错误的话初始值为1
                UserLoginCheckDto userLoginCheck = new UserLoginCheckDto();
                userLoginCheck.ErrorCount = 1;
                userLoginCheck.LastForbiddenTime = DateTime.Now;
                this._cacheService.Set(cacheKey, userLoginCheck, CachingExpirationType.RelativelyStable);
                result = userLoginCheck;
            }
            else
            {
                TimeSpan minuteSpan = new TimeSpan(DateTime.Now.Ticks - result.LastForbiddenTime.Value.Ticks);
                var PastMinutes = minuteSpan.TotalMinutes;
                //大于规定时间，直接清除缓存，此时是允许继续登录操作的
                if (PastMinutes >= LOGIN_TIME)
                {
                    this.ClearCache(username);
                    //正确可以直接登录，所以直接返回即可
                    if (state == true) { return result; }
                    UserLoginCheckDto userLoginCheck = new UserLoginCheckDto();
                    userLoginCheck.ErrorCount = 1;
                    userLoginCheck.LastForbiddenTime = DateTime.Now;
                    this._cacheService.Set(cacheKey, userLoginCheck, CachingExpirationType.RelativelyStable);
                    result = userLoginCheck;
                }
                else
                {
                    //未成功次数超过规定次数直接抛出，阻止登录
                    if (result.ErrorCount >= LOGIN_MAXCOUNT)
                    {
                        //次数已超出，不管正确还是错误都是抛出
                        throw new ApiException(string.Format("您的登录次数已超过限制，请在 {0} 分钟后重试！", Math.Ceiling(LOGIN_TIME - PastMinutes)));
                    }
                    else
                    {
                        //限制次数限制时间内登录，正确直接返回即可，api有清除缓存
                        if (state == true) { return result; }
                        //有缓存我们就讲count+1，并且将最后时间更新即可
                        ++result.ErrorCount;
                        result.LastForbiddenTime = DateTime.Now;
                        if (result.ErrorCount == 5)
                        {
                            var objectId = userId?.ToString() ?? username;
                        }
                        this._cacheService.Set(cacheKey, result, CachingExpirationType.RelativelyStable);

                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 清除账号登录缓存
        /// </summary>
        /// <param name="username">登录账号</param>
        public void ClearCache(string username)
        {
            var cacheKey = this.GetUsernameCacheKey(username);
            this._cacheService.Remove(cacheKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">登录账号</param>
        /// <returns></returns>
        private string GetUsernameCacheKey(string username)
        {
            var result = string.Format("Login_CACHE_Username_{0}", username);
            return result;
        }

        #endregion

        #region 谷歌校验码

        public Task<bool> CheckUserBindGoogleAuthenticator(long id)
        {
            return this._userRepository.CheckUserBindGoogleAuthenticator(id);
        }

        public Task<bool> BindGoogleAuthenticator(long id, string secretKey)
        {
            if (secretKey.IsNull()) throw new ApiException("secretKey can't be null");
            return this._userRepository.BindGoogleAuthenticator(id, secretKey);
        }

        public Task<bool> ClearGoogleAuthenticator(long id)
        {
            return this._userRepository.ClearGoogleAuthenticator(id);
        }

        public bool CheckGoogleKey(string googleKey, string code, bool enableGoogleToken)
        {
            if (googleKey.IsNotNull() && code.IsNull() && enableGoogleToken) throw new ApiException("Please enter the google PIN!");
            if (googleKey.IsNotNull() && enableGoogleToken)
            {
                //GoogleAuthenticator googleAuthenticator = new GoogleAuthenticator();
                //if(!googleAuthenticator.ValidateTwoFactorPIN(googleKey, code, false))
                return false;
                //throw new ApiException("The verification code is wrong or expired.");
            }
            return true;
        }

        #endregion

        #region 判断

        public Task<List<BmsUser>> ListAsync()
        {
            return this._userRepository.Select.ToListAsync();
        }

        public Task<bool> AnyAsync()
        {
            return this._userRepository.Select.AnyAsync();
        }

        public async Task<IUser<long>> GetUser(long userId)
        {
            return await this._userRepository.GetAsync(userId);
        }

        public async Task<IUser<long>> GetUserByUserName(string username)
        {
            return await this._userRepository.GetUserByUserName(username);
        }
        #endregion

        private string GetCacheKey_GetUser(long userId)
        {
            return $"User:UserId:{userId}";
        }

        public async Task<IUser<long>> GetUserByCache(long userId)
        {
            var cacheKey = this.GetCacheKey_GetUser(userId);
            var user = await this._cacheService.LockGetAsync(cacheKey
               , () => this._userRepository.GetAsync(userId)
               , CachingExpirationType.ObjectCollection);
            return user;
        }

        public async Task<bool> UpdateUserInfoAsync(long userId, UserUpdateDto args)
        {
            var result = await _userRepository.ModifyAvatarAsync(userId, args);

            var cacheKey = this.GetCacheKey_GetUser(userId);
            this._cacheService.Remove(cacheKey);

            return result > 1;
        }
    }
}
