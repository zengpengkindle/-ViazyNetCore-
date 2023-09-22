using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Ddd;

public interface ICreateUpdateService<TEntityDto, in TKey>
    : ICreateUpdateService<TEntityDto, TKey, TEntityDto, TEntityDto>
{

}

public interface ICreateUpdateService<TEntityDto, in TKey, in TCreateUpdateDto>
    : ICreateUpdateService<TEntityDto, TKey, TCreateUpdateDto, TCreateUpdateDto>
{

}

public interface ICreateUpdateService<TGetOutputDto, in TKey, in TCreateUpdateDto, in TUpdateDto>
    : ICreateService<TGetOutputDto, TCreateUpdateDto>,
        IUpdateService<TGetOutputDto, TKey, TUpdateDto>
{

}