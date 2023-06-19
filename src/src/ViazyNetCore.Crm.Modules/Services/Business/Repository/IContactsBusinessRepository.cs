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
    public interface IContactsBusinessRepository : IBaseRepository<CrmContactsBusiness, long>
    {
        Task<PageData<BusinessContactListDto>> GetContactPageList(Pagination pagination, long businessId);
        Task<PageData<BusinessContractListDto>> GetContractPageList(Pagination pagination, long businessId);
    }
}
