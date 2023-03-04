using System.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.ShopMall.AppApi
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;
        private readonly ILockProvider _lockProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string _memberId => this._httpContextAccessor.HttpContext!.User.GetUserId();
        public AddressController(AddressService addressService, ILockProvider lockProvider, IHttpContextAccessor httpContextAccessor)
        {
            this._addressService = addressService;
            this._lockProvider = lockProvider;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<AddressModel>> FindAddress()
        {
            var result = await this._addressService.GetMemberAddress(_memberId);

            return result;
        }


        public async Task<string> SubimtAddress(AddressModel model)
        {
            if (model.Id.IsNull())
            {
                var result = await this._addressService.AddAddress(_memberId, model);
                return result;
            }
            else
            {
                await this._addressService.UpdateAddress(_memberId, model.Id, model);
                return model.Id;
            }
        }

        public async Task<bool> RemoveAddress(string addressId)
        {
            await this._addressService.RemoveAddress(_memberId, addressId);
            return true;
        }
    }
}
