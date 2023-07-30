using FreeSql;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    [Injection]
    public class FormFieldValueRepository : DefaultRepository<FormFieldValue, long>, IFormFieldValueRepository
    {
        public FormFieldValueRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
