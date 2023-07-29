using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.TunnelWorks.Modules.Mapper
{
    public class TemplateProfile : Profile
    {
        public TemplateProfile()
        {
            CreateMap<FormTemplate, FormTemplateDto>();
            CreateMap<FormTemplateDto, FormTemplate>();
        }
    }
}
