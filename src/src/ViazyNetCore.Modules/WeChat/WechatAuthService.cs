using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Essensoft.Paylink.WeChatPay.V3.Domain;
using ViazyNetCore.Caching;
using ViazyNetCore.Modules.Internal;
using ViazyNetCore.Modules.Models;
using ViazyNetCore.Modules.ShopMall;

namespace ViazyNetCore.Modules
{
    [Injection]
    public class WechatAuthService : IWechatAuthService
    {
        private readonly IThridAuthUserRepository _thridAuthUserRepository;
        private readonly IThirdAuthAppInfoRepository _thirdAuthAppInfoRepository;
        private readonly IMemberService _memberService;
        private readonly ICacheService _cacheService;

        public WechatAuthService(IThridAuthUserRepository thridAuthUserRepository
            , IThirdAuthAppInfoRepository thirdAuthAppInfoRepository
            , IMemberService memberService
            , ICacheService cacheService)
        {
            this._thridAuthUserRepository = thridAuthUserRepository;
            this._thirdAuthAppInfoRepository = thirdAuthAppInfoRepository;
            this._memberService = memberService;
            this._cacheService = cacheService;
        }

        public static string GetWxAuthCodeCacheKey(string authCode) => $"wxauthcode:{authCode}";

        public async Task<WechatAuthDto> Auth(WechatAuthUpdateDto updateDto)
        {
            var thridAuthUser = await this._thridAuthUserRepository.Where(p => p.UnionId == updateDto.UnionId).FirstAsync();
            var authCode = Guid.NewGuid().ToString("N");
            var authCodeKey = GetWxAuthCodeCacheKey(authCode);
            if (thridAuthUser == null)
            {
                return new WechatAuthDto
                {
                    IsBindMobile = false,
                    OpenId = updateDto.OpenId,
                    AuthCode = authCode
                };
            }

            var appInfo = await GetOrAddAuthAppInfo(new AuthAppInfoEditDto
            {
                AppId = updateDto.AppId,
                OpenId = updateDto.OpenId,
                UnionId = updateDto.UnionId,
            });

            var member = await this._memberService.GetInfoByMemberId(thridAuthUser.MemberId);
            if (member == null)
            {
                throw new ApiException("登陆认证失败，请联系客服");
            }
            if (member.Status != ComStatus.Enabled)
            {
                throw new ApiException("系统检测到您的访问行为异常，已限制登陆");
            }
            if (!member.IsBindMobile)
            {
                this._cacheService.Set(authCodeKey, updateDto, CachingExpirationType.RelativelyStable);
                return new WechatAuthDto
                {
                    IsBindMobile = false,
                    OpenId = appInfo.OpenId,
                    AuthCode = authCode
                };
            }

            return new WechatAuthDto()
            {
                MemberId = member.Id,
                GetUserProfile = member.NickName.IsNull() && member.AvatarUrl.IsNull(),
                OpenId = appInfo.OpenId,
                IsBindMobile = member.IsBindMobile
            };
        }

        public async Task<WechatAuthDto> BindMobile(BindMobileDto bindModel)
        {
            if (bindModel.Mobile.IsNull())
            {
                throw new ApiException("请填写手机号码");
            }
            if (bindModel.AuthCode.IsNull())
            {
                throw new ApiException("授权过期，请重试");
            }
            var model = await GetWechatAuthCodeInfo(bindModel.AuthCode);
            if (model == null)
            {
                throw new ApiException("授权过期，请重试");
            }

            var thirdAuthAppInfo = await this._thirdAuthAppInfoRepository.Where(w => w.UnionId == model.UnionId)
             .FirstAsync();
            Member? member = null;
            var isNewMember = false;
            if (thirdAuthAppInfo == null || thirdAuthAppInfo.MemberId.IsNull())
            {
                member = await this._memberService.GetByMobile(bindModel.Mobile);
                if (member == null)
                {
                    isNewMember = true;
                    member = new Member
                    {
                        Id = Snowflake<Member>.NextIdString(),
                        Salt = Guid.NewGuid(),
                        AvatarUrl = null,
                        LastLoginTime = DateTime.Now,
                        NickName = null,
                        Username = model.UnionId,
                        CreateTime = DateTime.Now,
                        Mobile = bindModel.Mobile,
                        Status = ComStatus.Enabled
                    };
                    await this._memberService.InsertAsync(member);
                }
                else
                {
                    if (await this._thridAuthUserRepository.Where(p => p.UnionId != model.UnionId && p.MemberId == member.Id).AnyAsync())
                    {
                        throw new ApiException("该手机号已绑定了其他微信账号,请使用手机登录或关联的微信登录");
                    }
                    // 否则直接关联现有的手机账号；
                }

                if (thirdAuthAppInfo == null)
                {
                    thirdAuthAppInfo = new ThirdAuthAppInfo
                    {
                        AppId = model.AppId,
                        CreateTime = DateTime.Now,
                        IsSubscribe = 0,
                        MemberId = member.Id,
                        UnionId = model.UnionId,
                        OpenId = model.OpenId,
                        Type = UserAccountTypes.WxApp
                    };

                    await this._thirdAuthAppInfoRepository.InsertAsync(thirdAuthAppInfo);
                }
                else if (thirdAuthAppInfo.MemberId.IsNull())
                {
                    await this._thirdAuthAppInfoRepository.UpdateDiy
                        .Where(p => p.Id == thirdAuthAppInfo.Id).Set(p => p.MemberId == member.Id)
                        .ExecuteAffrowsAsync();
                }
            }
            else
            {
                member = await this._memberService.GetInfoByMemberId(thirdAuthAppInfo.MemberId);
                if (!member.IsBindMobile)
                {
                    await this._memberService.UpdateMobile(thirdAuthAppInfo.MemberId, bindModel.Mobile);
                }
            }

            var appInfo = await GetOrAddAuthAppInfo(new AuthAppInfoEditDto
            {
                AppId = model.AppId,
                OpenId = model.OpenId,
                UnionId = model.UnionId,
            });

            return new WechatAuthDto()
            {
                MemberId = member.Id,
                GetUserProfile = isNewMember ? true : (member.NickName.IsNull() && member.AvatarUrl.IsNull()),
                OpenId = appInfo.OpenId,
                IsBindMobile = member.IsBindMobile
            };
        }

        public async Task<ThirdAuthAppInfo> GetOrAddAuthAppInfo(AuthAppInfoEditDto editDto)
        {
            var appInfo = await this._thirdAuthAppInfoRepository
                .Where(w => w.UnionId == editDto.UnionId && w.Type == UserAccountTypes.WxApp && w.AppId == editDto.AppId)
                .FirstAsync();
            if (appInfo == null)
            {
                appInfo = new ThirdAuthAppInfo()
                {
                    AppId = editDto.AppId,
                    UnionId = editDto.UnionId,
                    CreateTime = DateTime.Now,
                    IsSubscribe = 0,
                    OpenId = editDto.OpenId,
                    Type = UserAccountTypes.WxApp,
                };

                await this._thirdAuthAppInfoRepository.InsertAsync(appInfo);
            }
            return appInfo;
        }

        public Task<WechatAuthUpdateDto?> GetWechatAuthCodeInfo(string authCode)
        {
            var authCodeKey = GetWxAuthCodeCacheKey(authCode);
            return Task.FromResult(this._cacheService.Get<WechatAuthUpdateDto>(authCodeKey));
        }
    }
}
