using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.ShopMall.Manage.Application.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    [Area("shopmall")]
    public class CreditController : ControllerBase
    {
        private readonly CreditService _creditService;

        public CreditController(CreditService creditService)
        {
            this._creditService = creditService;
        }
        [HttpPost]
        public async Task<bool> AddCredit(CreditModel credit)
        {
            await this._creditService.CreateCredit(credit.Name, credit.Key, credit.CreditType);
            return true;
        }

        [HttpPost]
        public async Task<bool> ModifyStatus(string id, ComStatus status)
        {
            await this._creditService.ModifyCreditStatus(id, status);
            return true;
        }

        [HttpPost]
        public Task<PageData<Credits>> FindAll(Pagination args)
        {
            return this._creditService.FindAll(args);
        }

        [HttpPost]
        public Task<Dictionary<string, string>> GetAllAsync()
        {
            return this._creditService.GetAllAsync();
        }
    }

    public class CreditModel
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public CreditType CreditType { get; set; }
    }
}
