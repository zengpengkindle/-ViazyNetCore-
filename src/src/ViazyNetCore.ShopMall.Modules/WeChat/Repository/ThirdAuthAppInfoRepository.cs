namespace ViazyNetCore.Modules.Repository
{
    [Injection]
    public class ThirdAuthAppInfoRepository : DefaultRepository<ThirdAuthAppInfo, long>, IThirdAuthAppInfoRepository
    {
        public ThirdAuthAppInfoRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
