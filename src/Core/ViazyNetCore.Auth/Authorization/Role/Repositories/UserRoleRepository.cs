namespace ViazyNetCore.Authorization.Modules.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IFreeSql _fsql;

        public UserRoleRepository(IFreeSql fsql)
        {
            this._fsql = fsql;
        }

        public async Task AddUserToRoles(string userId, List<string> roleIds)
        {
            var userRole = this._fsql.Select<BmsUserRole>();
            await userRole.Where(p => p.UserId == userId).ToDelete().ExecuteAffrowsAsync();
            foreach (var roleId in roleIds)
            {
                var userrole = new BmsUserRole()
                {
                    Id = Snowflake.NextIdString(),
                    UserId = userId,
                    RoleId = roleId
                };
                await this._fsql.Insert(userrole).ExecuteAffrowsAsync();
            }
        }
    }
}
