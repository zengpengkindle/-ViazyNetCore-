using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Moq;
using Moq.Protected;
using ViazyNetCore.Authrozation;
using Xunit;
using ViazyNetCore.ShopMall.Manage.Application.Controllers;

namespace ViazyNetCore.Manage.WebApiTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var httpMessageHanlderMocker = new Mock<HttpMessageHandler>();

            httpMessageHanlderMocker.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                            ItExpr.IsAny<HttpRequestMessage>(),
                            ItExpr.IsAny<CancellationToken>())
                            .ReturnsAsync(new HttpResponseMessage
                            {
                                StatusCode = System.Net.HttpStatusCode.OK,
                                Content = new StringContent("Here text your response message.")
                            });
            HttpClient httpClient = new HttpClient(httpMessageHanlderMocker.Object);
            var response = await httpClient.GetAsync("https://baidu.com");

            Assert.True(response.IsSuccessStatusCode);
        }
        [Fact]
        public void HttpContextAccessorTest()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var context = new DefaultHttpContext();

            httpContextAccessor.Setup(a => a.HttpContext).Returns(context);
        }

        [Fact]
        public void HttpContextUserTest()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "example name"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim("custom-claim", "example claim value"),
                }, "mock"));

            var controller = new CommonController();
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }
    }
}