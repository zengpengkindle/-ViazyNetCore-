using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.Authorization.Models;

namespace ViazyNetCore.TunnelWorks.Modules.Repository
{
    [Injection]
    public class VehicleRepository : DefaultRepository<Vehicle, long>, IVehicleRepository
    {
        public VehicleRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public async Task<VehicleInfoDto> GetInfoAsync(long id)
        {
            return await this.Select.Where(p => p.Id == id)
                //.From<VehicleCategory, BmsOrg>((v, vc, o) =>
                //v.LeftJoin(v => v.CatId == vc.Id).LeftJoin(p => p.OrgId == o.Id)
                //)
                .WithTempQuery(t => new VehicleInfoDto
                {
                    Id = t.Id,
                    VehicleImg = t.VehicleImg,
                    CatId = t.CatId,
                    //CatName = t.t2.Name,
                    Code = t.Code,
                    EngineCode = t.EngineCode,
                    FrameCode = t.FrameCode,
                    LicenseCode = t.LicenseCode,
                    OrgId = t.OrgId,
                    OriginEnterprise = t.OriginEnterprise,
                    //OrgName = t.t3.Name,
                    OriginPlace = t.OriginPlace,
                    Spec = t.Spec,
                    VehicleStatus = t.VehicleStatus,
                })
                  .FirstAsync();
        }

        public async Task<PageData<VehicleListItemDto>> PageListAsync(PaginationSort pagination, VehicleQueryDto queryDto)
        {
            var result = await this.Select
                .From<VehicleCategory, BmsOrg>((v, vc, o) =>
                v.LeftJoin(v => v.CatId == vc.Id).LeftJoin(p => p.OrgId == o.Id)
                )
                 .WhereIf(queryDto.OriginPlace.IsNotNull(), t => t.t1.OriginPlace == queryDto.OriginPlace)
                 .WhereIf(queryDto.Code.IsNotNull(), t => t.t1.Code.Contains(queryDto.Code))
                 .WhereIf(queryDto.EngineCode.IsNotNull(), t => t.t1.EngineCode.Contains(queryDto.EngineCode))
                 .WhereIf(queryDto.FrameCode.IsNotNull(), t => t.t1.FrameCode.Contains(queryDto.FrameCode))
                 .WhereIf(queryDto.LicenseCode.IsNotNull(), t => t.t1.LicenseCode.Contains(queryDto.LicenseCode))
                 .WhereIf(queryDto.CatId != 0, t => t.t1.CatId == t.t1.CatId)
                 .WhereIf(queryDto.OrgId != 0, t => t.t1.OrgId == t.t1.OrgId)
                 .PageSort(out var page, (sortField, sort, fsql) =>
                 {
                     return fsql;
                 }, pagination)
                 .WithTempQuery(t => new VehicleListItemDto
                 {
                     CatId = t.t1.CatId,
                     CatName = t.t2.Name,
                     Code = t.t1.Code,
                     CreateTime = t.t1.CreateTime,
                     EngineCode = t.t1.EngineCode,
                     FrameCode = t.t1.FrameCode,
                     Id = t.t1.Id,
                     LicenseCode = t.t1.LicenseCode,
                     OrgId = t.t1.OrgId,
                     OriginEnterprise = t.t1.OriginEnterprise,
                     OrgName = t.t3.Name,
                     OriginPlace = t.t1.OriginPlace,
                     Spec = t.t1.Spec,
                     VehicleStatus = t.t1.VehicleStatus,
                 }).ToListAsync();

            return new PageData<VehicleListItemDto>
            {
                Rows = result,
                Total = page.Count,
            };
        }

        public Task UpsertAsync(Vehicle enitity)
        {
            return this.Orm.Insert<Vehicle>().AppendData(enitity)
                 .OnDuplicateKeyUpdate()
                 .ExecuteAffrowsAsync();
        }
    }
}
