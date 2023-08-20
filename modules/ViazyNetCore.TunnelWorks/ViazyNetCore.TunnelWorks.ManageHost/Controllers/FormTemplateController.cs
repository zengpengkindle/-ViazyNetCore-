using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.TunnelWorks.ManageHost.Controllers
{
    [Route("api/formTemplate")]
    public class FormTemplateController : BaseController
    {
        private readonly FormTemplateService _formTemplateService;
        private readonly FormFieldService _formFieldService;
        private readonly IMapper _mapper;

        public FormTemplateController(FormTemplateService formTemplateService
            , FormFieldService formFieldService
            , IMapper mapper)
        {
            this._formTemplateService = formTemplateService;
            this._formFieldService = formFieldService;
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

        [HttpGet, Route("getInfo")]
        public async Task<FormTemplateEditResult> GetForm(long id)
        {
            var result = await this._formTemplateService.GetForm(id);
            return this._mapper.Map<FormTemplateDto, FormTemplateEditResult>(result);
        }

        [HttpPost, Route("edit")]
        public async Task EditForm([FromBody] FormTemplateEditRequest request)
        {
            await this._formTemplateService.EditForm(new FormTemplateEditDto
            {
                Description = request.Description,
                FormType = request.FormType,
                Id = request.Id,
                Name = request.Name,
                PublichStatus = FormStatus.Create,
                SourceId = 0,
                SourceType = FormSourceType.Default,
                Status = ComStatus.Enabled
            });
        }

        [HttpGet, Route("getFields")]
        public async Task<List<FormWidgetResult>> GetFormFields(long id)
        {
            var result = await _formFieldService.GetListByFormId(id);
            return this._mapper.Map<List<FormWidgetDto>, List<FormWidgetResult>>(result);
        }

        [HttpPost, Route("saveFields")]
        public async Task SaveFormFields(long formId, [FromBody] List<FormWidgetResult> formWidgets)
        {
            var dtos = this._mapper.Map<List<FormWidgetResult>, List<FormWidgetDto>>(formWidgets);
            await this._formFieldService.SaveFormFields(formId, dtos);
        }

        [HttpPost, Route("saveForms")]
        public async Task SaveFormValues(long formId, [FromBody] List<FormFieldValueEditRequest> editRequest)
        {
            var editDtos = this._mapper.Map<List<FormFieldValueEditRequest>, List<FormFieldValueDto>>(editRequest);
            var batchId = Snowflake<FormFieldValue>.NextId();
            await this._formFieldService.SaveForms(batchId, editDtos);
        }
    }
}
