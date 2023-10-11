using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ViazyNetCore.CmsKit.ViewModels;

namespace ViazyNetCore.CmsKit.Mapper
{
    public class DictionaryProfile : Profile
    {
        public DictionaryProfile()
        {
            CreateMap<DictionaryValue, DictionaryValueViewResult>();
        }
    }
}
