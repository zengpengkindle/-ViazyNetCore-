using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ViazyNetCore.Ddd;

public abstract class CurdService<TEntity, TEntityDto, TKey>
    : CurdService<TEntity, TEntityDto, TKey, TKey>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IDto<TKey>
{
    protected CurdService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class CurdService<TEntity, TEntityDto, TKey, TQueryDto>
    : CurdService<TEntity, TEntityDto, TKey, TQueryDto, TEntityDto>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IDto<TKey>
{
    protected CurdService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class CurdService<TEntity, TEntityDto, TKey, TQueryDto, TCreateDto>
    : CurdService<TEntity, TEntityDto, TKey, TQueryDto, TCreateDto, TCreateDto>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IDto<TKey>
{
    protected CurdService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class CurdService<TEntity, TEntityDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    : CurdService<TEntity, TEntityDto, TEntityDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IDto<TKey>
{
    protected CurdService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class CurdService<TEntity, TGetOutputDto, TGetListItemDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    : AbstractKeyCurdService<TEntity, TGetOutputDto, TGetListItemDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity<TKey>
    where TGetOutputDto : IDto<TKey>
    where TGetListItemDto : IDto<TKey>
{
    protected new IBaseRepository<TEntity, TKey> Repository { get; }

    protected CurdService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {
        Repository = repository;
    }

    protected override async Task DeleteByIdAsync(TKey id)
    {
        await Repository.DeleteAsync(id);
    }

    protected override async Task<TEntity> GetEntityByIdAsync(TKey id)
    {
        return await Repository.GetAsync(id);
    }

    protected override void MapToEntity(TUpdateDto updateInput, TEntity entity)
    {
        if (updateInput is IDto<TKey> entityDto)
        {
            entityDto.Id = entity.Id;
        }

        base.MapToEntity(updateInput, entity);
    }
}