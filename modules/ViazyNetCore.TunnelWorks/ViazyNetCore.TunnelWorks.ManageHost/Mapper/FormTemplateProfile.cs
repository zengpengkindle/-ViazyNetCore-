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

            CreateMap<FormWidgetDto, FormWidgetResult>();
            CreateMap<FormWidgetOptionDto, FormWidgetOptionResult>();
        }
    }
}
