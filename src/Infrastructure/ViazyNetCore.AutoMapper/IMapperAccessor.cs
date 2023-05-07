using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ViazyNetCore.AutoMapper
{
    public interface IMapperAccessor
    {
        IMapper Mapper { get; set; }
    }

    internal class MapperAccessor : IMapperAccessor
    {
        public IMapper Mapper { get; set; }
    }

}
