using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Authorization.Repositories;
using ViazyNetCore.Caching;

namespace ViazyNetCore.Authorization
{
    [Injection]
    public class OrgService : IOrgService
    {
        private readonly IOrgRepository _orgRepository;
        private readonly IUserOrgRepository _userOrgRepository;
        private readonly ICacheService _cacheService;

        public OrgService(IOrgRepository orgRepository, IUserOrgRepository userOrgRepository, ICacheService cacheService)
        {
            this._orgRepository = orgRepository;
            this._userOrgRepository = userOrgRepository;
            this._cacheService = cacheService;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrgGetDto> GetAsync(long id)
        {
            var result = await _orgRepository.Where(p => p.Id == id).ToOneAsync<OrgGetDto>();
            return result;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<OrgListDto>> GetListAsync(string key)
        {
            var data = await _orgRepository
                .WhereIf(key.IsNotNull(), a => a.Name.Contains(key) || a.Code.Contains(key))
                .OrderBy(a => a.ParentId)
                .OrderBy(a => a.Sort)
                .ToListAsync<OrgListDto>();

            return data;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(OrgAddDto input)
        {
            if (await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Name == input.Name))
            {
                throw new ApiException($"此部门已存在");
            }

            if (input.Code.IsNotNull() && await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Code == input.Code))
            {
                throw new ApiException($"此部门编码已存在");
            }

            var entity = input.CopyTo<BmsOrg>();

            if (entity.Sort == 0)
            {
                var sort = await _orgRepository.Select.Where(a => a.ParentId == input.ParentId).MaxAsync(a => a.Sort);
                entity.Sort = sort + 1;
            }
            entity.CreateTime = DateTime.Now;
            await _orgRepository.InsertAsync(entity);
            //await this._cacheService.Remove(GetCacheKey_GetDataPermission("*"));

            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(OrgUpdateDto input)
        {
            var entity = await _orgRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw new ApiException("部门不存在");
            }

            if (input.Id == input.ParentId)
            {
                throw new ApiException("上级部门不能是本部门");
            }

            if (await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Name == input.Name))
            {
                throw new ApiException($"此部门已存在");
            }

            if (input.Code.IsNotNull() && await _orgRepository.Select.AnyAsync(a => a.ParentId == input.ParentId && a.Id != input.Id && a.Code == input.Code))
            {
                throw new ApiException($"此部门编码已存在");
            }

            var childIdList = await _orgRepository.GetChildIdListAsync(input.Id);
            if (childIdList.Contains(input.ParentId))
            {
                throw new ApiException($"上级部门不能是下级部门");
            }

            input.CopyTo(entity);
            await _orgRepository.UpdateAsync(entity);

            //await this._cacheService.RemoveByPatternAsync(GetCacheKey_GetDataPermission("*"));
        }

        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            //本部门下是否有员工
            if (await _userOrgRepository.HasUser(id))
            {
                throw new ApiException($"当前部门有员工无法删除");
            }

            var orgIdList = await _orgRepository.GetChildIdListAsync(id);
            //本部门的下级部门下是否有员工
            if (await _userOrgRepository.HasUser(orgIdList))
            {
                throw new ApiException($"本部门的下级部门有员工无法删除");
            }

            //删除部门角色
            //await _roleOrgRepository.DeleteAsync(a => orgIdList.Contains(a.OrgId));

            //删除本部门和下级部门
            await _orgRepository.DeleteAsync(a => orgIdList.Contains(a.Id));

            //await this._cacheService.RemoveByPatternAsync(GetCacheKey_GetDataPermission("*"));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SoftDeleteAsync(long id)
        {
            //本部门下是否有员工
            if (await _userOrgRepository.HasUser(id))
            {
                throw new ApiException($"当前部门有员工无法删除");
            }

            var orgIdList = await _orgRepository.GetChildIdListAsync(id);
            //本部门的下级部门下是否有员工
            if (await _userOrgRepository.HasUser(orgIdList))
            {
                throw new ApiException($"本部门的下级部门有员工无法删除");
            }

            //删除部门角色
            //await _roleOrgRepository.SoftDeleteAsync(a => orgIdList.Contains(a.OrgId));

            //删除本部门和下级部门
            await _orgRepository.UpdateDiy.Where(a => orgIdList.Contains(a.Id))
                .Set(p => p.Status, ComStatus.Deleted)
                .ExecuteAffrowsAsync();

            await this._cacheService.RemoveByPatternAsync(GetCacheKey_GetDataPermission("*"));
        }

        private string GetCacheKey_GetDataPermission(string userId)
        {
            return $"CacheKey:permission:{userId}";
        }
    }
}
