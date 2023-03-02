namespace ViazyNetCore.Authorization.Modules.Repositories
{
    [Injection]
    public interface IUserRoleRepository : IBaseRepository<BmsUserRole, string>
    {
        Task UpdateUserToRoles(string userId, List<string> roleIds);
        Task<List<string>?> GetRoleIdsOfUser(string userId);
    }
}
