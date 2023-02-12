namespace ViazyNetCore.Authorization.Modules.Repositories
{
    [Injection]
    public class UserRoleRepository :DefaultRepository<BmsUserRole, string>,  IUserRoleRepository
    {
        public UserRoleRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public async Task AddUserToRoles(string userId, List<string> roleIds)
        {
            var userRole = this.Select;
            await userRole.Where(p => p.UserId == userId).ToDelete().ExecuteAffrowsAsync();
            foreach (var roleId in roleIds)
            {
                var userrole = new BmsUserRole()
                {
                    Id = Snowflake.NextIdString(),
                    UserId = userId,
                    RoleId = roleId
                };
                await this.Orm.Insert(userrole).ExecuteAffrowsAsync();
            }
        }

        public Task<List<string>?> GetRoleIdsOfUser(string userId)
        {
            return this.Select.From<BmsRole>().InnerJoin((ur, r) => ur.RoleId == r.Id)
                .Where((ur, r) => ur.UserId == userId).ToListAsync(p => p.t2.Name);
        }
    }
}
