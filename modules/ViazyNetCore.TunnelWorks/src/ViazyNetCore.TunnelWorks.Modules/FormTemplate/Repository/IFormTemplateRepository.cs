using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.TunnelWorks.Models;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    [Injection]
    public interface IFormTemplateRepository : IBaseRepository<FormTemplate, long>
    {
        Task<PageData<FormTemplateDto>> GetPageListAsync(Pagination pagination, FormTemplateListQueryDto queryDto);
    }
}
