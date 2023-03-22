using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.CO2NET;
using Senparc.Weixin;
using Senparc.CO2NET.AspNet;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.MP;
using Senparc.Weixin.Cache.Redis;
using Senparc.Weixin.WxOpen;

namespace ViazyNetCore.ShopMall.AppApi.Extensions
{
    public static class SenparcConfigurationExtensions
    {
        public static void AddSenparc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSenparcWeixinServices(configuration);
        }

        public static void UseSenparc(this WebApplication app, IWebHostEnvironment env, IConfiguration configuration)
        {
            var senparcSetting = configuration.GetSection("SenparcSetting").Get<SenparcSetting>();
            var senparcWeixinSetting = configuration.GetSection("SenparcWeixinSetting").Get<SenparcWeixinSetting>();


            app.UseSenparcGlobal(env, senparcSetting, globalRegister =>
            {
                //Senparc.CO2NET.Cache.Redis.Register.SetConfigurationOption(senparcSetting.Cache_Redis_Configuration);
                //Senparc.CO2NET.Cache.Redis.Register.UseKeyValueRedisNow();
            }, true)
            .UseSenparcWeixin(senparcWeixinSetting, (weixinRegister, _) =>
            {
                //weixinRegister.UseSenparcWeixinCacheRedis();

                weixinRegister.RegisterMpAccount(senparcWeixinSetting, "【ViazyNetCore】公众号");
                weixinRegister.RegisterWxOpenAccount(senparcWeixinSetting, "【ViazyNetCore】小程序");
                //weixinRegister.RegisterTenpayRealV3(senparcWeixinSetting, "【猿推推】微信支付");
            });
        }

    }
}
