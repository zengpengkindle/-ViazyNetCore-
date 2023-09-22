using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Ddd;

public interface ICreateService<TEntityDto>
    : ICreateService<TEntityDto, TEntityDto>
{

}

public interface ICreateService<TGetOutputDto, in TCreateInput>
{
    Task<TGetOutputDto> CreateAsync(TCreateInput input);
}

