using Xunit;
using ViazyNetCore.ShopMall.Manage.Application.Controllers.ShopMall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using ViazyNetCore.Modules.ShopMall;
using Microsoft.AspNetCore.Mvc;

namespace ViazyNetCore.ShopMall.Manage.Application.Controllers.ShopMall.Tests
{
    public class ProductCatControllerTests
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProductCatController _productCatController;

        public ProductCatControllerTests(IServiceProvider serviceProvider, IHttpContextAccessor httpContextAccessor, IMapper mapper, ProductCatService productCatService)
        {
            this._serviceProvider = serviceProvider;
            this._httpContextAccessor = httpContextAccessor;
            this._productCatController = new ProductCatController(mapper, productCatService);
            this._productCatController.ControllerContext = new ControllerContext()
            {
                HttpContext = _httpContextAccessor.HttpContext
            };
        }

        [Fact()]
        public void ProductCatControllerTest()
        {
        }

        [Fact()]
        public async Task EditTest()
        {
            await this._productCatController.Edit(new ViewModel.CatEditReq
            {
                Exdata = "",
                Id = null,
                Image = "",
                IsHidden = false,
                Name = "Test",
                ParentId = null,
                Sort = 1,
                Status = ComStatus.Enabled
            });
        }

        [Fact()]
        public void FindPageListTest()
        {

        }

        [Fact()]
        public void GetAllListTest()
        {

        }

        [Fact()]
        public void GetTest()
        {

        }
    }
}