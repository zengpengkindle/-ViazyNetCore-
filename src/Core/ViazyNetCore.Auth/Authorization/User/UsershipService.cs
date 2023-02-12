using Microsoft.Extensions.Options;
using ViazyNetCore;
using ViazyNetCore.Auth;
using ViazyNetCore.Modules;

namespace ViazyNetCore.Authorization.Modules
{
    public class UsershipService : IUsershipService
    {
        private readonly UserOption _userOption;
        private readonly IUserRepository _userRepository;

        public UsershipService(IUserRepository userRepository, IOptions<UserOption> options)
        {
            this._userOption = options.Value;
            this._userRepository = userRepository;
        }

        public Task ActivateUsers(IEnumerable<string> userIds, ComStatus status = ComStatus.Enabled)
        {
            return this._userRepository.ActivateUsers(userIds, status);
        }

        public async Task<bool> ChangePassword(string username, string password, string newPassword)
        {
            await this.ValidateUser(username, password);

            //var userId = await this._userRepository.GetUserIdByUserName(username);
            return await this.ResetPassword(username, newPassword);
        }

        public Task<IUser> CreateUser(IUser user, string password)
        {
            return this.CreateUser(user, password, string.Empty, string.Empty, false);
        }


        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user">待创建的用户</param>
        /// <param name="password">密码</param>
        /// <param name="passwordQuestion">密码问题</param>
        /// <param name="passwordAnswer">密码答案</param>
        /// <param name="ignoreDisallowedUsername">是否忽略禁用的用户名称</param>
        /// <param name="userCreateStatus">用户帐号创建状态</param>
        /// <returns>创建成功返回IUser，创建失败返回null</returns>
        public async Task<IUser> CreateUser(IUser user, string password, string passwordQuestion, string passwordAnswer, bool ignoreDisallowedUsername)
        {
            var user_object = user as BmsUser;
            if (user_object == null)
            {
                UserCreateExceptions.Unknown();
            }

            //user_object.PasswordFormat = this._userOption.UserPasswordFormat;
            user_object.PasswordSalt = Guid.NewGuid();
            user_object.Password = UserPasswordHelper.EncodePassword(password, user_object.PasswordSalt, this._userOption.UserPasswordFormat);

            user = await this._userRepository.CreateUser(user_object, ignoreDisallowedUsername);

            return user;
        }


        public async Task DeleteUser(string userId, string takeOverUserName, bool isTakeOver, bool deleteContent = false)
        {
            var user = await this.GetUserByUserIdAsync(userId);
            if (user == null)
                UserDeleteExceptions.DeletingUserNotFound();

            if (isTakeOver)
            {
                string takeOverUserId = await this._userRepository.GetUserIdByUserName(takeOverUserName);
                var takeOverUser = this.GetUserByUserIdAsync(takeOverUserId);
                if (takeOverUser == null)
                    UserDeleteExceptions.InvalidTakeOverUsername();
            }

            user.Status = ComStatus.Deleted;
            await this._userRepository.UpdateAsync(user);
        }

        public async Task<bool> ResetPassword(string username, string newPassword)
        {
            var userId = await this._userRepository.GetUserIdByUserName(username);
            var user = await this.GetUserByUserIdAsync(userId);
            if (user == null)
                return false;

            string storedPassword = UserPasswordHelper.EncodePassword(newPassword, user.PasswordSalt, UserPasswordFormat.MD5);
            bool result = await this._userRepository.ResetPassword(user, storedPassword);


            return result;
        }

        public async Task UpdateUser(IUser user, IUser historyData = null)
        {
            var history_user_object = historyData as BmsUser;
            if (!(user is BmsUser user_object))
                return;
            await this._userRepository.UpdateDiy.SetDto(new { user.Id, user.Username, user.Nickname }).ExecuteAffrowsAsync();
        }

        public async Task<BmsUser> ValidateUser(string username, string password)
        {
            string userId = await this._userRepository.GetUserIdByUserName(username);

            //var mobileRegex = new Regex("^1[3-9]\\d{9}$");
            //var emailRegex = new Regex("^([a-zA-Z0-9_.-]+)@([0-9A-Za-z.-]+).([a-zA-Z.]{2,6})$");
            var loginUser = await this.GetUserByUserIdAsync(userId);
            if (loginUser == null)
                throw new ApiException("用户名密码不匹配", (int)UserLoginStatus.InvalidCredentials);

            if (!UserPasswordHelper.CheckPassword(password, loginUser.Password, loginUser.PasswordSalt, UserPasswordFormat.MD5))
                throw new ApiException("用户名密码不匹配", (int)UserLoginStatus.InvalidCredentials);

            if (loginUser.Status != ComStatus.Enabled)
                throw new ApiException("账户未激活", (int)UserLoginStatus.NotActivated);

            return loginUser;
        }

        public Task<BmsUser> GetUserByUserIdAsync(string userId)
        {
            return this._userRepository.GetAsync(userId);
        }
    }
}
