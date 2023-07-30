using FreeSql;
using ViazyNetCore.TunnelWorks.Models;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    [Injection]
    public class FormFieldSortRepository : DefaultRepository<FormFieldSort, long>, IFormFieldSortRepository
    {
        public FormFieldSortRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
