using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace ViazyNetCore.TunnelWorks.Modules
{
    [Injection]
    public class FormFieldService
    {
        private readonly IFormFieldRepository _formFieldRepository;
        private readonly IMapper _mapper;

        public FormFieldService(IFormFieldRepository formFieldRepository
            , IMapper mapper)
        {
            this._formFieldRepository = formFieldRepository;
            this._mapper = mapper;
        }

        public async Task<List<FormWidgetDto>> GetListByFormId(long formId)
        {
            var fields = await _formFieldRepository.GetListByFormIdAsync(formId);
            var widgetDtos = new List<FormWidgetDto>();
            foreach(var field in fields)
            {
                widgetDtos.Add(new FormWidgetDto
                {
                    Id = field.FieldName,
                    FormItemFlag = true,
                    Icon = "",
                    Key = field.Name,
                    Options = JSON.Parse<FormWidgetOptionDto>(field.Options),
                    Type = field.Type.ToString().Replace("_", "-").ToLower(),
                });
            }

            return widgetDtos;
        }

        public async Task SaveFormFields(long formId, List<FormWidgetDto> dtos)
        {
            //var entities = new List<FormField>();
            var sort = 1;
            foreach(var item in dtos)
            {
                Enum.TryParse<WeightType>(item.Type.Replace("-", "_"), true, out var fielType);
                var fieldEntity = new FormField
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    CreateUserName = "",
                    DefaultValue = null,
                    ExamineCategoryId = 0,
                    FieldName = item.Id,
                    Id = 0,
                    FieldType = (int)fielType,
                    Sorting = sort++,
                    Type = fielType,
                    FormId = formId,
                    InputTips = item.Options.Placeholder,
                    IsNull = !item.Options.Required,
                    Lable = item.Options.Label,
                    Name = item.Options.Name,
                    Options = JSON.Stringify(item.Options),
                };
                await this._formFieldRepository.Orm.InsertOrUpdate<FormField>().SetSource(fieldEntity).ExecuteAffrowsAsync();
            }
        }
    }
}
