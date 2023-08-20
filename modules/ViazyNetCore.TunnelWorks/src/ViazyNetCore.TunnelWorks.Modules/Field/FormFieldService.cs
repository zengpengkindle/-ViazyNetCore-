﻿using System;
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
        private readonly IFormFieldValueRepository _formFieldValueRepository;
        private readonly IMapper _mapper;

        public FormFieldService(IFormFieldRepository formFieldRepository
            , IFormFieldValueRepository formFieldValueRepository
            , IMapper mapper)
        {
            this._formFieldRepository = formFieldRepository;
            this._formFieldValueRepository = formFieldValueRepository;
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
                    Id = Snowflake.NextId(),
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    CreateUserName = "",
                    DefaultValue = null,
                    ExamineCategoryId = 0,
                    FieldName = item.Id,
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

        public async Task SaveForms(long batchId, List<FormFieldValueDto> editDtos)
        {
            var fieldValueEntities = new List<FormFieldValue>();
            foreach(var item in editDtos)
            {
                var fieldEntity = new FormFieldValue
                {
                    Id = Snowflake.NextId(),
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    CreateUserName = "",
                    FieldId = item.FieldId,
                    Value = item.Value,
                    BatchId = batchId,
                    Name = null
                };
                fieldValueEntities.Add(fieldEntity);
            }
            await this._formFieldValueRepository.Orm.InsertOrUpdate<FormFieldValue>().SetSource(fieldValueEntities).ExecuteAffrowsAsync();
        }
    }
}
