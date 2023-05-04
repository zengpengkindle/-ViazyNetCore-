using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ViazyNetCore.Authorization.Modules;
using ViazyNetCore.Authorization.Repositories;
using ViazyNetCore.Data.FreeSql.Extensions;
using ViazyNetCore.Modules;

namespace ViazyNetCore.OpenIddict.Domain
{
    public partial class IdentityUserStore
        : IUserStore<IdentityUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public IdentityUserStore(IUserRepository userRepository
            , IUserService userService
            , IMapper mapper)
        {
            this._userRepository = userRepository;
            this._userService = userService;
            this._mapper = mapper;
        }


        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            await this._userRepository.InsertAsync(user, cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Check.NotNull(user, nameof(user));

            await this._userRepository.DeleteAsync(user.Id, cancellationToken);

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await this._userService.GetUser(userId.ParseTo<long>());
            return this._mapper.Map<IdentityUser>(user);
        }


        public async Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await this._userService.GetUserByUserName(normalizedUserName);
            return this._mapper.Map<IdentityUser>(user);
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
