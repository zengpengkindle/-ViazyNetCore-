using Xunit;
using ViazyNetCore.Manage.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ViazyNetCore.Modules.ShopMall;
using System.Providers;
using ViazyNetCore.ShopMall.Manage.Application.Controllers;

namespace ViazyNetCore.Manage.WebApi.Controllers.Tests
{
    public class TradeControllerTests
    {
        private readonly TradeController _tradeController;
        public TradeControllerTests(IServiceProvider serviceProvider
            , IHttpContextAccessor httpContextAccessor
            , TradeService tradeService
            , ILockProvider lockProvider
            , LogisticsService logisticsService)
        {
            this._tradeController = new TradeController(tradeService, lockProvider, logisticsService, httpContextAccessor);
            this._tradeController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = httpContextAccessor.HttpContext!
            };
        }

        [Fact()]
        public async Task FindAllTest()
        {
            await this._tradeController.FindAll(new TradePageArgments
            {

            });
        }

        [Fact()]
        public void FindTradeTest()
        {
            throw new NotImplementedException();
        }
    }
}