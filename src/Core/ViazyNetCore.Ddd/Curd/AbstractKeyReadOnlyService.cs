using AutoMapper;

namespace ViazyNetCore.Ddd;

public abstract class AbstractKeyReadOnlyService<TEntity, TEntityDto, TKey>
    : AbstractKeyReadOnlyService<TEntity, TEntityDto, TEntityDto, TKey, TKey>
    where TEntity : class, IEntity
{
    protected AbstractKeyReadOnlyService(IBaseRepository<TEntity> repository, IMapper mapper)
        : base(repository, mapper)
    {

    }
}

public abstract class AbstractKeyReadOnlyService<TEntity, TEntityDto, TKey, TQueryDto>
    : AbstractKeyReadOnlyService<TEntity, TEntityDto, TEntityDto, TKey, TQueryDto>
    where TEntity : class, IEntity
{
    protected AbstractKeyReadOnlyService(IBaseRepository<TEntity> repository, IMapper mapper)
        : base(repository, mapper)
    {

    }
}

public abstract class AbstractKeyReadOnlyService<TEntity, TGetOutputDto, TGetListItemDto, TKey, TQueryDto>
    : ApplicationService
    , IReadOnlyService<TGetOutputDto, TGetListItemDto, TKey, TQueryDto>
    where TEntity : class, IEntity
{
    protected IBaseRepository<TEntity> ReadOnlyRepository { get; }

    protected virtual string? GetPolicyName { get; set; }

    protected virtual string? GetListPolicyName { get; set; }

    protected AbstractKeyReadOnlyService(IBaseRepository<TEntity> repository, IMapper mapper) : base(mapper)
    {
        ReadOnlyRepository = repository;
    }

    public virtual async Task<TGetOutputDto> GetAsync(TKey id)
    {
        await CheckGetPolicyAsync();

        var entity = await GetEntityByIdAsync(id);

        return await MapToGetOutputDtoAsync(entity);
    }

    public virtual async Task<PageData<TGetListItemDto>> GetListAsync(TQueryDto input, PaginationSort pagination)
    {
        await CheckGetListPolicyAsync();

        var query = CreateFilteredQuery(input);
        var totalCount = await query.CountAsync();

        var entityDtos = new List<TGetListItemDto>();

        if (totalCount > 0)
        {
            query = ApplySorting(query, pagination);
            var entities = await query.ToListAsync();
            entityDtos = await MapToGetListOutputDtosAsync(entities);
        }

        return new PageData<TGetListItemDto>(
            entityDtos,
            totalCount
        );
    }
    protected virtual ISelect<TEntity> ApplySorting(ISelect<TEntity> query, PaginationSort pagination)
    {
        return query;
    }

    protected virtual ISelect<TEntity> ApplyPaging(ISelect<TEntity> query, PaginationSort pagination)
    {
        return query.Page(pagination.Page, pagination.Limit);
    }

    protected abstract Task<TEntity> GetEntityByIdAsync(TKey id);

    protected virtual async Task CheckGetPolicyAsync()
    {
        await CheckPolicyAsync(GetPolicyName);
    }

    protected virtual async Task CheckGetListPolicyAsync()
    {
        await CheckPolicyAsync(GetListPolicyName);
    }

    protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TQueryDto input)
    {
        //No sorting
        return query;
    }

    protected virtual ISelect<TEntity> CreateFilteredQuery(TQueryDto input)
    {
        return ReadOnlyRepository.Select;
    }

    protected virtual Task<TGetOutputDto> MapToGetOutputDtoAsync(TEntity entity)
    {
        return Task.FromResult(MapToGetOutputDto(entity));
    }

    protected virtual TGetOutputDto MapToGetOutputDto(TEntity entity)
    {
        return ObjectMapper.Map<TEntity, TGetOutputDto>(entity);
    }

    protected virtual async Task<List<TGetListItemDto>> MapToGetListOutputDtosAsync(List<TEntity> entities)
    {
        //var dtos = new List<TGetListOutputDto>();

        //foreach (var entity in entities)
        //{
        //    dtos.Add(await MapToGetListOutputDtoAsync(entity));
        //}
        var dtos = this.ObjectMapper.Map<List<TEntity>, List<TGetListItemDto>>(entities);
        return dtos;
    }

    protected virtual Task<TGetListItemDto> MapToGetListOutputDtoAsync(TEntity entity)
    {
        return Task.FromResult(MapToGetListOutputDto(entity));
    }

    protected virtual TGetListItemDto MapToGetListOutputDto(TEntity entity)
    {
        return this.ObjectMapper.Map<TEntity, TGetListItemDto>(entity);
    }
}
