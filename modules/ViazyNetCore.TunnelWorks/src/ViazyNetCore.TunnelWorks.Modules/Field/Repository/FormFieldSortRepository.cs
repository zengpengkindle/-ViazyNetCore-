using FreeSql;
using ViazyNetCore.TunnelWorks.Models;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    public class FormFieldSortRepository : DefaultRepository<FormFieldSort, long>, IFormFieldSortRepository
    {
        public FormFieldSortRepository(IFreeSql fsql) : base(fsql)
        {
        }
    }
}
