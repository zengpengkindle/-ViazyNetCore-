using FreeSql;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ViazyNetCore.Data.FreeSql
{
    public class RepositoryBase<TEntity, TKey> : DefaultRepository<TEntity, TKey>, IRepositoryBase<TEntity, TKey> where TEntity : class
    {
        public IUser User { get; set; }

        public RepositoryBase(IFreeSql fsql) : base(fsql) { }
        public RepositoryBase(IFreeSql fsql, Expression<Func<TEntity, bool>> filter) : base(fsql, filter) { }
        public RepositoryBase(IFreeSql fsql, UnitOfWorkManager uowManger) : base(uowManger?.Orm ?? fsql)
        {
            uowManger?.Binding(this);
        }

        public virtual Task<TDto> GetAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToOneAsync<TDto>();
        }

        public virtual Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync<TDto>();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync();
        }

        public virtual async Task<bool> SoftDeleteAsync(TKey id)
        {
            await UpdateDiy
                .SetDto(new
                {
                    IsDeleted = true,
                    ModifiedUserId = User.Id,
                    ModifiedUserName = User.Username
                })
                .WhereDynamic(id)
                .ExecuteAffrowsAsync();

            return true;
        }

        public virtual async Task<bool> SoftDeleteAsync(TKey[] ids)
        {
            await UpdateDiy
                .SetDto(new
                {
                    IsDeleted = true,
                    ModifiedUserId = User.Id,
                    ModifiedUserName = User.Username
                })
                .WhereDynamic(ids)
                .ExecuteAffrowsAsync();

            return true;
        }

        public virtual async Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames)
        {
            await UpdateDiy
                .SetDto(new
                {
                    IsDeleted = true,
                    ModifiedUserId = User.Id,
                    ModifiedUserName = User.Username
                })
                .Where(exp)
                .DisableGlobalFilter(disableGlobalFilterNames)
                .ExecuteAffrowsAsync();

            return true;
        }

        public virtual async Task<bool> DeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames)
        {
            await Select
            .Where(exp)
            .DisableGlobalFilter(disableGlobalFilterNames)
            .AsTreeCte()
            .ToDelete()
            .ExecuteAffrowsAsync();

            return true;
        }

        public virtual async Task<bool> SoftDeleteRecursiveAsync(Expression<Func<TEntity, bool>> exp, params string[] disableGlobalFilterNames)
        {
            await Select
            .Where(exp)
            .DisableGlobalFilter(disableGlobalFilterNames)
            .AsTreeCte()
            .ToUpdate()
            .SetDto(new
            {
                IsDeleted = true,
                ModifiedUserId = User.Id,
                ModifiedUserName = User.Username
            })
            .ExecuteAffrowsAsync();

            return true;
        }
    }

    public class RepositoryBase<TEntity> : RepositoryBase<TEntity, long>, IRepositoryBase<TEntity> where TEntity : class
    {
        public RepositoryBase(string db, UnitOfWorkManagerCloud uowm) : this(uowm.GetUnitOfWorkManager(db)) { }
        RepositoryBase(UnitOfWorkManager uowm) : base(uowm.Orm)
        {
            uowm.Binding(this);
        }
    }
}