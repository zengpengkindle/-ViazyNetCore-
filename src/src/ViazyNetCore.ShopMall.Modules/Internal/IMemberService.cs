namespace ViazyNetCore.Modules
{
    public interface IMemberService
    {
        Task<List<string>> GetMemberIdsByMemberNameLike(string nameLike);

        Task<string> GetMemberNameByMemberId(string memberId);
        Task<Member> GetInfoByMemberId(string memberId);

        Task<string> GetMemberIdByUsername(string username);
        Task<Member> GetByMobile(string mobile);
        Task InsertAsync(Member member);
        Task UpdateMobile(string memberId, string mobile);
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
            return this._engine.Select<Member>().Where(p => p.NickName.Contains(nameLike)).WithTempQuery(p => p.Id).ToListAsync();
        }

        public Task<Member> GetInfoByMemberId(string memberId)
        {
            return this._engine.GetRepository<Member>().Where(p => p.Id == memberId).FirstAsync();
        }

        public Task<string> GetMemberNameByMemberId(string memberId)
        {
            return this._engine.Select<Member>().Where(p => p.Id == memberId).WithTempQuery(p => p.NickName).FirstAsync();
        }

        public Task<Member> GetByMobile(string mobile)
        {
            return this._engine.Select<Member>().Where(p => p.Mobile == mobile).FirstAsync();
        }

        public Task InsertAsync(Member member)
        {
            return this._engine.GetRepository<Member>().InsertAsync(member);
        }

        public Task UpdateMobile(string memberId, string mobile)
        {
            return this._engine.GetRepository<Member>().UpdateDiy
                .Where(p => p.Id == memberId)
                .Set(p => p.Mobile == mobile)
                .ExecuteAffrowsAsync();
        }
    }
}
