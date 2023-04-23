using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ViazyNetCore.Model.Crm;
using ViazyNetCore.Modules.Models;
using ViazyNetCore.Modules.Repository;

namespace ViazyNetCore.Modules
{
    public class BusinessService
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IBusinessChangeRepository _businessChangeRepository;
        private readonly IContactsBusinessRepository _contactsBusinessRepository;
        private readonly IBusinessProductRepository _businessProductRepository;
        private readonly RecordService _recordService;
        private readonly IMapper _mapper;

        public BusinessService(IBusinessRepository businessRepository
            , IBusinessChangeRepository businessChangeRepository
            , IContactsBusinessRepository contactsBusinessRepository
            , IBusinessProductRepository businessProductRepository
            , RecordService recordService
            , IMapper mapper)
        {
            this._businessRepository = businessRepository;
            this._businessChangeRepository = businessChangeRepository;
            this._contactsBusinessRepository = contactsBusinessRepository;
            this._businessProductRepository = businessProductRepository;
            this._recordService = recordService;
            this._mapper = mapper;
        }

        public async Task AddOrUpdate(BusinessEditDto editDto)
        {
            if (editDto.BatchId.IsNull())
                editDto.BatchId = Snowflake.NextIdString();
            var business = this._mapper.Map<BusinessEditDto, CrmBusiness>(editDto);
            long businessId = business.Id;
            await this._recordService.UpdateRecord(editDto.Feilds, editDto.BatchId);
            if (editDto.Id > 0)
            {
                var oldBusiness = await this._businessRepository.GetAsync(editDto.Id);
                if (oldBusiness.StatusId != editDto.StatusId)
                {
                    var change = new CrmBusinessChange()
                    {
                        BusinessId = editDto.Id,
                        StatusId = editDto.StatusId,
                    };
                    await this._businessChangeRepository.InsertAsync(change);
                }
                await this._businessRepository.UpdateAsync(business);
            }
            else
            {
                await this._businessRepository.InsertAsync(business);
                if (editDto.ContactsId > 0)
                {
                    await this._contactsBusinessRepository.InsertAsync(new CrmContactsBusiness
                    {
                        BusinessId = business.Id,
                        ContactsId = editDto.ContactsId
                    });
                    businessId = business.Id;
                    await this._recordService.AddRecord(business.Id, CrmType.Business);
                }
            }
            if (editDto.Products != null)
            {
                var products = this._mapper.Map<List<BusinessProductDto>, List<CrmBusinessProduct>>(editDto.Products);
                foreach (var item in products)
                {
                    item.BussinessId = businessId;
                    await this._businessProductRepository.InsertAsync(item);
                }
            }
        }

        public async Task<BusinessRecordDto> QueryById(long businessId)
        {
            var entity = await this._businessRepository.GetAsync(businessId);
            if (entity == null)
                throw new ApiException("请求记录不存在");
            var busineessDto = this._mapper.Map<CrmBusiness, BusinessRecordDto>(entity);

            return busineessDto;
        }
    }
}
