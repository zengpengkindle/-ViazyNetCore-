using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Ddd;

public interface IReadOnlyService<TEntityDto, in TKey>
    : IReadOnlyService<TEntityDto, TEntityDto, TKey, TKey>
{

}

public interface IReadOnlyService<TEntityDto, in TKey, in TQueryDto>
    : IReadOnlyService<TEntityDto, TEntityDto, TKey, TQueryDto>
{

}

public interface IReadOnlyService<TGetOutputDto, TGetListItemDto, in TKey, in TQueryDto>
{
    Task<TGetOutputDto> GetAsync(TKey id);

    Task<PageData<TGetListItemDto>> GetListAsync(TQueryDto input, PaginationSort pagination);
}
