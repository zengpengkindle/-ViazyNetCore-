using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Ddd;

public interface ICurdService<TEntityDto, in TKey>
    : ICurdService<TEntityDto, TKey, PaginationSort>
{

}

public interface ICurdService<TEntityDto, in TKey, in TGetListInput>
    : ICurdService<TEntityDto, TKey, TGetListInput, TEntityDto>
{

}

public interface ICurdService<TEntityDto, in TKey, in TGetListInput, in TCreateInput>
    : ICurdService<TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
{

}

public interface ICurdService<TEntityDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
    : ICurdService<TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
{

}
public interface ICurdService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
{

}
