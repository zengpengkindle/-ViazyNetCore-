namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public class WeChatAccessTokenRepository : DefaultRepository<WeChatAccessToken, int>, IWeChatAccessTokenRepository
    {
        public WeChatAccessTokenRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
