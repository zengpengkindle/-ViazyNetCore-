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
    public class FormTemplateRepository : DefaultRepository<FormTemplate, long>, IFormTemplateRepository
    {
        public FormTemplateRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public Task<PageData<FormTemplateDto>> GetPageListAsync(Pagination pagination, FormTemplateListQueryDto queryDto)
        {
            return this.Select.WhereIf(queryDto.NameLike.IsNotNull(), p => p.Name.Contains(queryDto.NameLike))
                .WhereIf(queryDto.Status != null, p => p.Status == queryDto.Status)
                .WhereIf(queryDto.FormType != null, p => p.FormType == queryDto.FormType)
                .WithTempQuery(p => new FormTemplateDto
                {
                    Id = p.Id,
                    FormType = p.FormType,
                    Status = p.Status,
                    Description = p.Description,
                    Name = p.Name,
                    PublichStatus = p.PublichStatus,
                    SourceId = p.SourceId,
                    SourceType = p.SourceType
                })
                .ToPageAsync(pagination);
        }
    }
}
