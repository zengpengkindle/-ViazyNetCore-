using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Ddd;

public interface IUpdateService<TEntityDto, in TKey>
    : IUpdateService<TEntityDto, TKey, TEntityDto>
{

}

public interface IUpdateService<TGetOutputDto, in TKey, in TUpdateInput>
{
    Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input);
}

