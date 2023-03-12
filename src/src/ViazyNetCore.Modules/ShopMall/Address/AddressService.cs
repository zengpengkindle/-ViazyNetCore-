using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class AddressService
    {
        private readonly IFreeSql _fsql;
        private readonly ILogger<AddressService> _logger;

        public AddressService(IFreeSql fsql, ILogger<AddressService> logger)
        {
            this._fsql = fsql;
            this._logger = logger;
        }

        /// <summary>
        /// 获取收货地址
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public async Task<AddressModel> GetAddress(string addressId)
        {
            return await this._fsql.Select<MemberAddress>()
                .Where(p => p.Id == addressId)
                .WithTempQuery(p => new AddressModel
                {
                    Id = p.Id,
                    Province = p.ReceiverProvince,
                    City = p.ReceiverCity,
                    County = p.ReceiverDistrict,
                    AddressDetail = p.ReceiverDetail,
                    Tel = p.ReceiverMobile,
                    Name = p.ReceiverName,
                    PostalCode = p.PostalCode,
                    AreaCode = p.AreaCode,
                    Address = p.ReceiverProvince + p.ReceiverCity + p.ReceiverDistrict + p.ReceiverDetail,
                    IsDefault = p.IsDefault
                }).ToOneAsync();
        }

        /// <summary>
        /// 获取会员默认收货地址
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public async Task<AddressModel> GetMemberDefaultAddress(string memberId)
        {
            if (this._fsql.Select<MemberAddress>().Any(p => p.MemberId == memberId && p.IsDefault))
            {
                return await this._fsql.Select<MemberAddress>()
                .Where(p => p.MemberId == memberId && p.IsDefault)
                .WithTempQuery(p => new AddressModel
                {
                    Id = p.Id,
                    Province = p.ReceiverProvince,
                    City = p.ReceiverCity,
                    County = p.ReceiverDistrict,
                    AddressDetail = p.ReceiverDetail,
                    Tel = p.ReceiverMobile,
                    Name = p.ReceiverName,
                    PostalCode = p.PostalCode,
                    AreaCode = p.AreaCode,
                    Address = p.ReceiverProvince + p.ReceiverCity + p.ReceiverDistrict + p.ReceiverDetail,
                    IsDefault = p.IsDefault
                }).FirstAsync();
            }
            else
            {
                return await this._fsql.Select<MemberAddress>()
              .Where(p => p.MemberId == memberId)
              .WithTempQuery(p => new AddressModel
              {
                  Id = p.Id,
                  Province = p.ReceiverProvince,
                  City = p.ReceiverCity,
                  County = p.ReceiverDistrict,
                  AddressDetail = p.ReceiverDetail,
                  Tel = p.ReceiverMobile,
                  Name = p.ReceiverName,
                  PostalCode = p.PostalCode,
                  AreaCode = p.AreaCode,
                  Address = p.ReceiverProvince + p.ReceiverCity + p.ReceiverDistrict + p.ReceiverDetail,
                  IsDefault = p.IsDefault
              }).FirstAsync();
            }
        }

        /// <summary>
        /// 获取收货地址
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public async Task<List<AddressModel>> GetMemberAddress(string memberId)
        {
            return await this._fsql.Select<MemberAddress>()
                .Where(p => p.MemberId == memberId)
                .WithTempQuery(p => new AddressModel
                {
                    Id = p.Id,
                    Province = p.ReceiverProvince,
                    City = p.ReceiverCity,
                    County = p.ReceiverDistrict,
                    AddressDetail = p.ReceiverDetail,
                    Tel = p.ReceiverMobile,
                    Name = p.ReceiverName,
                    PostalCode = p.PostalCode,
                    AreaCode = p.AreaCode,
                    Address = p.ReceiverProvince + p.ReceiverCity + p.ReceiverDistrict + p.ReceiverDetail,
                    IsDefault = p.IsDefault
                }).OrderByDescending(t => t.IsDefault).ToListAsync();
        }

        /// <summary>
        /// 新增收货地址
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task<string> AddAddress(string memberId, AddressModel address)
        {
            var memberAddress = new MemberAddress
            {
                Id = Snowflake<MemberAddress>.NextIdString(),
                ReceiverProvince = address.Province,
                ReceiverCity = address.City,
                ReceiverDistrict = address.County,
                ReceiverDetail = address.AddressDetail,
                ReceiverMobile = address.Tel,
                ReceiverName = address.Name,
                AreaCode = address.AreaCode,
                PostalCode = address.PostalCode,
                IsDefault = address.IsDefault,
                MemberId = memberId
            };

            if (address.IsDefault)
            {
                if (this._fsql.Select<MemberAddress>().Any(p => p.MemberId == memberId && p.IsDefault))
                {
                    await this._fsql.Select<MemberAddress>().Where(p => p.MemberId == memberId && p.IsDefault)
                        .ToUpdate()
                        .SetDto(new { IsDefault = false })
                        .ExecuteAffrowsAsync();
                }
            }

            await this._fsql.InsertOrUpdate<MemberAddress>().SetSource(memberAddress).ExecuteAffrowsAsync();
            return memberAddress.Id;
        }

        /// <summary>
        /// 移除用户收货地址
        /// </summary>
        /// <param name="addressId"></param>
        /// <returns></returns>
        public async Task RemoveAddress(string memberId, string addressId)
        {
            await this._fsql.Delete<MemberAddress>()
               .Where(p => p.Id == addressId && p.MemberId == memberId).ExecuteAffrowsAsync();
        }

        /// <summary>
        /// 修改收货地址
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="addressId"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task UpdateAddress(string memberId, string addressId, AddressModel address)
        {
            var memberAddress = new MemberAddress
            {
                Id = addressId,
                ReceiverProvince = address.Province,
                ReceiverCity = address.City,
                ReceiverDistrict = address.County,
                ReceiverDetail = address.AddressDetail,
                ReceiverMobile = address.Tel,
                ReceiverName = address.Name,
                PostalCode = address.PostalCode,
                AreaCode = address.AreaCode,
                IsDefault = address.IsDefault,
                MemberId = memberId
            };

            if (address.IsDefault)
            {
                if (this._fsql.Select<MemberAddress>().Any(p => p.MemberId == memberId && p.IsDefault && p.Id != addressId))
                {
                    await this._fsql.Update<MemberAddress>().Where(p => p.MemberId == memberId && p.IsDefault).SetDto(new { IsDefault = false }).ExecuteAffrowsAsync();
                }
            }
            await this._fsql.Update<MemberAddress>().SetDto(memberAddress).ExecuteAffrowsAsync();
        }
    }
}
