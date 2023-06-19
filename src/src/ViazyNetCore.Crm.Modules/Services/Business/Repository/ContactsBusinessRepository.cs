using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql;
using ViazyNetCore.Crm.Model;
using ViazyNetCore.Modules.Models;

namespace ViazyNetCore.Modules.Repository
{
    public class ContactsBusinessRepository : DefaultRepository<CrmContactsBusiness, long>, IContactsBusinessRepository
    {
        public ContactsBusinessRepository(IFreeSql fsql) : base(fsql)
        {
        }

        public Task<PageData<BusinessContactListDto>> GetContactPageList(Pagination pagination, long businessId)
        {
            return this.Orm.Select<CrmContacts, CrmContactsBusiness>().InnerJoin((con, cb) => con.Id == cb.ContactsId)
                  .Where((con, cb) => cb.BusinessId == businessId)
                  .WithTempQuery((con, cb) => new BusinessContactListDto
                  {
                      Id = con.Id,
                      Mobile = con.Mobile,
                      Name = con.Name,
                      Post = con.Post,
                  })
                  .ToPageAsync(pagination);
        }

        public Task<PageData<BusinessContractListDto>> GetContractPageList(Pagination pagination, long businessId)
        {
            return this.Orm.Select<CrmContract, CrmCustomer>().InnerJoin((con, cus) => con.CustomerId == cus.Id)
                  .Where((con, cus) => con.BusinessId == businessId)
                  .WithTempQuery((con, cus) => new BusinessContractListDto
                  {
                      Id = con.Id,
                      CheckStatus = con.CheckStatus,
                      ContractName = con.ContractName,
                      CustomerName = cus.CustomerName,
                      EndTime = con.EndTime,
                      Money = con.Money,
                      Num = con.Num,
                      StartTime = con.StartTime,
                  })
                  .ToPageAsync(pagination);
        }
    }
}
