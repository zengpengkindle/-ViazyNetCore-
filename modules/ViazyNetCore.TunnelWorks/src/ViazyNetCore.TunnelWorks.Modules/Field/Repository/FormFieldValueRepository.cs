using FreeSql;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    public class FormFieldValueRepository : DefaultRepository<FormFieldValue, long>, IFormFieldValueRepository
    {
        public FormFieldValueRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
