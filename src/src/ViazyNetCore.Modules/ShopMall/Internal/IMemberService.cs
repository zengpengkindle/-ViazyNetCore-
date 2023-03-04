namespace ViazyNetCore.Modules.ShopMall
{
    public interface IMemberService
    {
        Task<List<string>> GetMemberIdsByMemberNameLike(string nameLike);

        Task<string> GetMemberNameByMemberId(string memberId);

        Task<string> GetMemberIdByUsername(string username);
    }

    public class DefaultMemberService : IMemberService
    {
        private readonly IFreeSql _engine;

        public DefaultMemberService(IFreeSql engine)
        {
            this._engine = engine;
        }

        public Task<string> GetMemberIdByUsername(string username)
        {
            return this._engine.Select<Member>().Where(p => p.Username == username).WithTempQuery(p => p.Id).FirstAsync();
        }

        public Task<List<string>> GetMemberIdsByMemberNameLike(string nameLike)
        {
            return this._engine.Select<Member>().Where(p => p.Name.Contains(nameLike)).WithTempQuery(p => p.Id).ToListAsync();
        }

        public Task<string> GetMemberNameByMemberId(string memberId)
        {
            return this._engine.Select<Member>().Where(p => p.Id == memberId).WithTempQuery(p => p.Name).FirstAsync();
        }
    }
}
