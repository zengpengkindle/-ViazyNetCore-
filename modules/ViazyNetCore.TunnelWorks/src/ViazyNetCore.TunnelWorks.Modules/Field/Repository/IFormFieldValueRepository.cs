using FreeSql;
using ViazyNetCore.TunnelWorks.Models;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    [Injection]
    public interface IFormFieldValueRepository : IBaseRepository<FormFieldValue, long>
    {

    }
}
