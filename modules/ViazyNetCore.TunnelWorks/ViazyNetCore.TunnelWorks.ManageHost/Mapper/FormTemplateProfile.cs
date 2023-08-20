using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.ManageHost.Mapper
{
    public class FormTemplateProfile : Profile
    {
        public FormTemplateProfile()
        {
            CreateMap<FormTemplateDto, FormTemplateResult>();
            CreateMap<FormTemplateDto, FormTemplateEditResult>();
            CreateMap<FormTemplateQueryRequest, FormTemplateListQueryDto>();

            CreateMap<FormWidgetDto, FormWidgetResult>().ReverseMap();
            CreateMap<FormWidgetOptionDto, FormWidgetOptionResult>().ReverseMap();

            CreateMap<FormFieldValueEditRequest, FormFieldValueDto>()
                .ForMember(p => p.FieldId, cfg => cfg.MapFrom(p => p.FieldId))
                .ForMember(p => p.Value, cfg => cfg.MapFrom(p => p.Value));
        }
    }
}
