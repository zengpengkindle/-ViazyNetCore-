using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ViazyNetCore.Ddd;

public abstract class CrudService<TEntity, TEntityDto, TKey>
    : CrudService<TEntity, TEntityDto, TKey, TKey>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IDto<TKey>
{
    protected CrudService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class CrudService<TEntity, TEntityDto, TKey, TQueryDto>
    : CrudService<TEntity, TEntityDto, TKey, TQueryDto, TEntityDto>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IDto<TKey>
{
    protected CrudService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class CrudService<TEntity, TEntityDto, TKey, TQueryDto, TCreateDto>
    : CrudService<TEntity, TEntityDto, TKey, TQueryDto, TCreateDto, TCreateDto>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IDto<TKey>
{
    protected CrudService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class CrudService<TEntity, TEntityDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    : CrudService<TEntity, TEntityDto, TEntityDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity<TKey>
    where TEntityDto : IDto<TKey>
{
    protected CrudService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class CrudService<TEntity, TGetOutputDto, TGetListItemDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    : AbstractKeyCrudService<TEntity, TGetOutputDto, TGetListItemDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity<TKey>
    where TGetOutputDto : IDto<TKey>
    where TGetListItemDto : IDto<TKey>
{
    protected new IBaseRepository<TEntity, TKey> Repository { get; }

    protected CrudService(IBaseRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
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