using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Senparc.Weixin.WxOpen.Entities;
using Senparc.Weixin.WxOpen.Helpers;
using ViazyNetCore.Auth.Jwt;
using ViazyNetCore.Caching;
using ViazyNetCore.Modules.Models;

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
        private readonly ICacheService _cacheService;

        public WeChatController(ICacheService cacheService)
        {
            this._cacheService = cacheService;
        }

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
        [HttpPost, Route("wx/getJscode")]
        public async Task<JsCodeRes> JsCode2Session([FromBody] JsCodeReq request, [FromServices] IWechatAuthService wechatAuthService,
            [FromServices] TokenProvider tokenProvider)
        {
            if (request.Code.IsNull())
                throw new ApiException("参数错误");

            var result = await SnsApi.JsCode2JsonAsync(MiniAppId, MiniSecret, request.Code);
            if (result?.errcode == ReturnCode.请求成功)
            {
                this._cacheService.Set("Wechat:CodeOpenId", result.session_key, CachingExpirationType.RelativelyStable);
                var wechatAuthResult = await wechatAuthService.Auth(new WechatAuthUpdateDto
                {
                    AppId = MiniAppId,
                    OpenId = result.openid,
                    UnionId = result.unionid
                });

                if (wechatAuthResult.MemberId.IsNull())
                {
                    return new JsCodeRes
                    {
                        IsBindMobile = wechatAuthResult.IsBindMobile,
                        GetUserProfile = wechatAuthResult.GetUserProfile,
                        OpenId = result.openid,
                        OpType = 1,
                        Token = await tokenProvider.IssueToken(wechatAuthResult.MemberId.ToString(), wechatAuthResult.MemberId.ToString(), null)
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

        [HttpPost, Route("wx/bindmobile")]
        public async Task<JsCodeRes> BindMobileAndLogin([FromBody] WechatBindMobileReq request
            , [FromServices] IWechatAuthService wechatAuthService
            , [FromServices] TokenProvider tokenProvider)
        {
            if (request.AuthCode.IsNull())
            {
                throw new ApiException("参数错误");
            }
            if (request.EncryptedData.IsNull())
            {
                if (request.Mobile.IsNull())
                {
                    throw new ApiException("请输入手机号");
                }

                if (!request.Mobile.IsMobile())
                {
                    throw new ApiException("无效的手机号码");
                }
                if (request.SmsCode.IsNull())
                {
                    throw new ApiException("请输入验证码");
                }
                // 短信验证码验证
                // TODO
                throw new ApiException("短信验证码错误");
            }
            else
            {
                var wxcodeInfo = await wechatAuthService.GetWechatAuthCodeInfo(request.AuthCode);
                if (wxcodeInfo == null)
                {
                    throw new ApiException("微信授权已过期,请退出重新登陆");
                }
                if (request.EncryptedData.IsNull() || request.Iv.IsNull())
                {
                    throw new ApiException("参数错误");
                }
                var sessionKey = this._cacheService.Get<string>($"Wechat:CodeOpenId:{wxcodeInfo.OpenId}");
                if (sessionKey.IsNull())
                {
                    throw new ApiException("微信授权已过期");
                }
                var profileInfo = EncryptHelper.DecodeEncryptedDataToEntityEasy<DecodedPhoneNumber>(sessionKey, request.EncryptedData, request.Iv);
                request.Mobile = profileInfo.phoneNumber;
                if (request.Mobile.IsNull())
                {
                    throw new ApiException("微信授权手机失败，或微信未绑定手机");
                }
            }

            if (request.Mobile.IsNull())
            {
                throw new ApiException("请输入手机号");
            }

            var wechatAuthResult = await wechatAuthService.BindMobile(new BindMobileDto
            {
                AuthCode = request.AuthCode,
                Mobile = request.Mobile
            });
            if (wechatAuthResult.MemberId.IsNotNull())
            {
                return new JsCodeRes
                {
                    IsBindMobile = true,
                    GetUserProfile = wechatAuthResult.GetUserProfile,
                    OpenId = wechatAuthResult.OpenId,
                    OpType = 1,
                    Token = await tokenProvider.IssueToken(wechatAuthResult.MemberId, wechatAuthResult.MemberId, null)
                };
            }
            else
            {// 前端引导绑定手机
                return new JsCodeRes
                {
                    GetUserProfile = wechatAuthResult.GetUserProfile,
                    OpenId = wechatAuthResult.OpenId,
                    OpType = 1
                };
            }
        }
    }
}
