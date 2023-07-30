using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Modules
{
    [Injection]
    public class FormTemplateService
    {
        private readonly IFormTemplateRepository _formTemplateRepository;
        private readonly IMapper _mapper;

        public FormTemplateService(IFormTemplateRepository formTemplateRepository, IMapper mapper)
        {
            this._formTemplateRepository = formTemplateRepository;
            this._mapper = mapper;
        }

        public async Task<FormTemplateDto> GetForm(long id)
        {
            var result = await this._formTemplateRepository.GetAsync(id);
            return this._mapper.Map<FormTemplate, FormTemplateDto>(result);
        }

        public async Task<long> EditForm(FormTemplateEditDto formTemplateDto)
        {
            var entity = this._mapper.Map<FormTemplateEditDto, FormTemplate>(formTemplateDto);
            await this._formTemplateRepository.UpsertAsync(entity);
            return entity.Id;
        }

        public async Task<PageData<FormTemplateDto>> GetPageList(Pagination pagination, FormTemplateListQueryDto queryDto)
        {
            return await this._formTemplateRepository.GetPageListAsync(pagination, queryDto);
        }
    }
}
