using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.TunnelWorks.ManageHost.Controllers
{
    [Route("formTemplate")]
    public class FormTemplateController : BaseController
    {
        private readonly FormTemplateService _formTemplateService;
        private readonly IMapper _mapper;

        public FormTemplateController(FormTemplateService formTemplateService, IMapper mapper)
        {
            this._formTemplateService = formTemplateService;
            this._mapper = mapper;
        }

        [HttpPost, Route("list")]
        public async Task<PageData<FormTemplateResult>> GetPageData([FromBody] FormTemplateQueryRequest request)
        {
            var queryDto = this._mapper.Map<FormTemplateQueryRequest, FormTemplateListQueryDto>(request);
            var result = await this._formTemplateService.GetPageList(request, queryDto);
            return new PageData<FormTemplateResult>()
            {
                Total = result.Total,
                Rows = this._mapper.Map<List<FormTemplateDto>, List<FormTemplateResult>>(result.Rows)
            };
        }

        [HttpPost, Route("listTest")]
        public async Task GetPageDataTest()
        {
            throw new NotImplementedException();
        }

        [HttpPost, Route("add")]
        public async Task AddForm([FromBody] FormTemplateAddRequest request)
        {
            await this._formTemplateService.AddForm(new FormTemplateAddDto
            {
                Description = request.Description,
                FormType = request.FormType,
                Id = 0,
                Name = request.Name,
                PublichStatus = FormStatus.Create,
                SourceId = 0,
                SourceType = FormSourceType.Default,
                Status = ComStatus.Enabled
            });
        }
    }
}
