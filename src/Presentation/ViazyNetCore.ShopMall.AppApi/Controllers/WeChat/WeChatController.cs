using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using ViazyNetCore.Auth.Jwt;

namespace ViazyNetCore.ShopMall.AppApi.Controllers
{
    [Area("shopmall")]
    [Route("api/wechat")]
    public class WeChatController : ControllerBase
    {
        //微信小程序
        private static readonly string MiniAppId = Config.SenparcWeixinSetting.WxOpenAppId;

        private static readonly string MiniSecret = Config.SenparcWeixinSetting.WxOpenAppSecret;
        /// <summary>
        /// DES加解密的默认密钥
        /// </summary>
        private static string DefaultDesKey = "rolls.rc";

        /// <summary>
        /// 获取加密的unionid(仅客户端无缓存unionid时请求)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        [HttpPost, Route("wx/sec_unionid")]
        public async Task<UnionIdRes> GetUnionId([FromBody] JsCodeReq request)
        {
            var result = await SnsApi.JsCode2JsonAsync(MiniAppId, MiniSecret, request.Code);
            if (result?.errcode == ReturnCode.请求成功)
            {
                var secUnionId = DataSecurity.Encrypt(result.unionid, DefaultDesKey);
                return new UnionIdRes
                {
                    SecUnionId = secUnionId
                };
            }
            else
            {
                var errcode = result?.ErrorCodeValue ?? 0;
                switch (errcode)
                {
                    case 40029:
                        {
                            throw new ApiException("code无效");
                        };
                    case 45011:
                        {
                            throw new ApiException("频率限制");
                        };
                    case 40226:
                        {
                            throw new ApiException("高风险等级用户，小程序登录拦截");
                        };
                    default:
                        {
                            throw new UnauthorizedAccessException();
                        };
                }
            }
        }

        /// <summary>
        /// 微信小程序登录/注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        [HttpPost, Route("wx/code")]
        public async Task<JsCodeRes> JsCode2Session([FromBody] JsCodeReq request, [FromServices] IWeChatAccessTokenService wechatAuthService,
            [FromServices] TokenProvider tokenProvider)
        {
            if (request.Code.IsNull())
                throw new ApiException("参数错误");

            var result = await SnsApi.JsCode2JsonAsync(MiniAppId, MiniSecret, request.Code);
            if (result?.errcode == ReturnCode.请求成功)
            {
                var wechatAuthResult = await wechatAuthService.Auth(new WechatAuthModel
                {
                    AppId = MiniAppId,
                    OpenId = result.openid,
                    UnionId = result.unionid
                });

                if (wechatAuthResult.KolUserId > 0)
                {
                    return new JsCodeRes
                    {
                        IsBindMobile = wechatAuthResult.IsBindMobile,
                        GetUserProfile = wechatAuthResult.GetUserProfile,
                        OpenId = result.openid,
                        OpType = 1,
                        Token = tokenProvider.IssueToken(wechatAuthResult.KolUserId, wechatAuthResult.KolUserId)
                    };
                }
                else
                {
                    // 前端引导绑定手机
                    return new JsCodeRes
                    {
                        AuthCode = wechatAuthResult.AuthCode,
                        IsBindMobile = wechatAuthResult.IsBindMobile,
                        GetUserProfile = wechatAuthResult.GetUserProfile,
                        OpenId = result.openid,
                        OpType = 1
                    };
                }
            }
            else
            {
                var errcode = result?.ErrorCodeValue ?? 0;
                switch (errcode)
                {
                    case 40029:
                        {
                            throw new ApiException("code无效");
                        };
                    case 45011:
                        {
                            throw new ApiException("频率限制");
                        };
                    case 40226:
                        {
                            throw new ApiException("高风险等级用户，小程序登录拦截");
                        };
                    default:
                        {
                            throw new UnauthorizedAccessException();
                        };
                }
            }
        }

    }
}
