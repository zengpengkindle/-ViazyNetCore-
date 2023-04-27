namespace ViazyNetCore.Authorization.Modules.Repositories
{
    [Injection]
    public interface IUserRoleRepository : IBaseRepository<BmsUserRole, long>
    {
        Task UpdateUserToRoles(long userId, List<long> roleIds);
        Task<List<long>?> GetRoleIdsOfUser(long userId);
    }
}
