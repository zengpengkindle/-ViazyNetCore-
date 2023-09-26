using AutoMapper;

namespace ViazyNetCore.Ddd;

public abstract class AbstractKeyCurdService<TEntity, TEntityDto, TKey>
    : AbstractKeyCurdService<TEntity, TEntityDto, TKey, TKey>
    where TEntity : class, IEntity
{
    protected AbstractKeyCurdService(IBaseRepository<TEntity> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class AbstractKeyCurdService<TEntity, TEntityDto, TKey, TQueryDto>
    : AbstractKeyCurdService<TEntity, TEntityDto, TKey, TQueryDto, TEntityDto, TEntityDto>
    where TEntity : class, IEntity
{
    protected AbstractKeyCurdService(IBaseRepository<TEntity> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class AbstractKeyCurdService<TEntity, TEntityDto, TKey, TQueryDto, TCreateDto>
    : AbstractKeyCurdService<TEntity, TEntityDto, TKey, TQueryDto, TCreateDto, TCreateDto>
    where TEntity : class, IEntity
{
    protected AbstractKeyCurdService(IBaseRepository<TEntity> repository, IMapper mapper) : base(repository, mapper)
    {

    }
}

public abstract class AbstractKeyCurdService<TEntity, TEntityDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    : AbstractKeyCurdService<TEntity, TEntityDto, TEntityDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity
{
    protected AbstractKeyCurdService(IBaseRepository<TEntity> repository, IMapper mapper) : base(repository, mapper)
    {

    }

    protected override Task<TEntityDto> MapToGetListOutputDtoAsync(TEntity entity)
    {
        return MapToGetOutputDtoAsync(entity);
    }

    protected override TEntityDto MapToGetListOutputDto(TEntity entity)
    {
        return MapToGetOutputDto(entity);
    }
}

public abstract class AbstractKeyCurdService<TEntity, TGetOutputDto, TGetListItemDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    : AbstractKeyReadOnlyService<TEntity, TGetOutputDto, TGetListItemDto, TKey, TQueryDto>,
        ICurdService<TGetOutputDto, TGetListItemDto, TKey, TQueryDto, TCreateDto, TUpdateDto>
    where TEntity : class, IEntity
{
    protected IBaseRepository<TEntity> Repository { get; }

    protected virtual string? CreatePolicyName { get; set; }

    protected virtual string? UpdatePolicyName { get; set; }

    protected virtual string? DeletePolicyName { get; set; }

    protected AbstractKeyCurdService(IBaseRepository<TEntity> repository, IMapper mapper)
        : base(repository, mapper)
    {
        Repository = repository;
    }

    public virtual async Task<TGetOutputDto> CreateAsync(TCreateDto input)
    {
        await CheckCreatePolicyAsync();

        var entity = await MapToEntityAsync(input);

        await Repository.InsertAsync(entity);

        return await MapToGetOutputDtoAsync(entity);
    }

    public virtual async Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateDto input)
    {
        await CheckUpdatePolicyAsync();

        var entity = await GetEntityByIdAsync(id);
        await MapToEntityAsync(input, entity);
        await Repository.UpdateAsync(entity);

        return await MapToGetOutputDtoAsync(entity);
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        await CheckDeletePolicyAsync();

        await DeleteByIdAsync(id);
    }

    protected abstract Task DeleteByIdAsync(TKey id);

    protected virtual async Task CheckCreatePolicyAsync()
    {
        await CheckPolicyAsync(CreatePolicyName);
    }

    protected virtual async Task CheckUpdatePolicyAsync()
    {
        await CheckPolicyAsync(UpdatePolicyName);
    }

    protected virtual async Task CheckDeletePolicyAsync()
    {
        await CheckPolicyAsync(DeletePolicyName);
    }

    protected virtual Task<TEntity> MapToEntityAsync(TCreateDto createInput)
    {
        return Task.FromResult(MapToEntity(createInput));
    }

    protected virtual TEntity MapToEntity(TCreateDto createInput)
    {
        var entity = ObjectMapper.Map<TCreateDto, TEntity>(createInput);
        return entity;
    }

    protected virtual Task MapToEntityAsync(TUpdateDto updateInput, TEntity entity)
    {
        MapToEntity(updateInput, entity);
        return Task.CompletedTask;
    }

    protected virtual void MapToEntity(TUpdateDto updateInput, TEntity entity)
    {
        ObjectMapper.Map(updateInput, entity);
    }
}
