using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.VisualBasic;
using ViazyNetCore.Identity.Domain;
using ViazyNetCore.IdentityService4.FreeSql.Models;
using ViazyNetCore.IdentityService4.SampleWeb.ViewModels;

namespace ViazyNetCore.IdentityService4.SampleWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _eventService;
        private readonly IdentityUserManager _userManager;
        private readonly SignInManager _signInManager;

        public AccountController(IIdentityServerInteractionService interactionService
            , IClientStore clientStore
            , IAuthenticationSchemeProvider schemeProvider
            , IEventService eventService
            , IdentityUserManager identityUserManager
            , SignInManager signInManager)
        {
            this._interactionService = interactionService;
            this._clientStore = clientStore;
            this._schemeProvider = schemeProvider;
            this._eventService = eventService;
            this._userManager = identityUserManager;
            this._signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl)
        {
            var vm = await BuildLoginViewModelAsync(returnUrl);
            //if(vm.IsExternalLoginOnly)
            //{
            //    return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
            //}

            return View(vm);
        }

        /// <summary>
        /// 登录请求
        /// </summary>
        /// <param name="model"></param>
        /// <param name="button"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interactionService.GetAuthorizationContextAsync(model.ReturnUrl);
            // the user clicked the "cancel" button
            if(button != "login")
            {
                if(context == null)
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Json(new LoginResult()
                    {
                        IsRedirect = true,
                        RedirectUrl = "/",
                    });
                }
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interactionService.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                //if(context.IsNativeClient())
                //{
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                //return this.LoadingPage("Redirect", model.ReturnUrl);
                //}
                return Json(
                    new LoginResult()
                    {
                        IsRedirect = true,
                        RedirectUrl = model.ReturnUrl,
                    });
            }

            var user = await this._userManager.FindByNameAsync(model.Username);
            if(user == null)
            {
                throw new ApiException("用户不存在");
            }

            var signInResult = await this._signInManager.CheckPasswordSignInAsync(user, model.Password, true);
            if(!signInResult.Succeeded)
            {
                if(signInResult.IsLockedOut)
                {
                    throw new ApiException($"用户因密码错误次数过多而被锁定 {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} 分钟，请稍后重试");
                }
                if(signInResult.IsNotAllowed)
                {
                    throw new ApiException("不允许登录。");
                }
                throw new ApiException("登录失败，用户名或账号无效。");
            }
            AuthenticationProperties props = null;
            if(model.RememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(7))
                };
            };
            var isuser = new IdentityServerUser(user.Id.ToString())
            {
                DisplayName = user.Nickname,
                AuthenticationTime = DateTime.Now,
            };
            await HttpContext.SignInAsync(isuser, props);
            if(context != null)
            {
                return Json(new LoginResult()
                {
                    IsRedirect = true,
                    RedirectUrl = model.ReturnUrl,
                });
            }

            if(Url.IsLocalUrl(model.ReturnUrl))
            {
                return Json(new LoginResult()
                {
                    IsRedirect = true,
                    RedirectUrl = model.ReturnUrl,
                });
            }
            else if(string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Json(new LoginResult()
                {
                    IsRedirect = true,
                    RedirectUrl = "/",
                });
            }
            else
            {
                // user might have clicked on a malicious link - should be logged
                throw new ApiException("登录成功，但系统无法针对你所在的系统进行跳转，请检查传入的URL。");
            }
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);

            if(vm.ShowLogoutPrompt == false)
            {
                // if the request for logout was properly authenticated from IdentityServer, then
                // we don't need to show the prompt and can just log the user out directly.
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // build a model so the logged out page knows what to display
            var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

            if(User?.Identity.IsAuthenticated == true)
            {
                // delete local authentication cookie
                await HttpContext.SignOutAsync();

                await _eventService.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

                if(!string.IsNullOrEmpty(User.GetSubjectId()) && long.TryParse(User.GetSubjectId(), out long userId))
                {
                    //写离线日志
                }
            }

            // check if we need to trigger sign-out at an upstream identity provider
            if(vm.TriggerExternalSignout)
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
            }

            return View("LoggedOut", vm);
        }


        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interactionService.GetAuthorizationContextAsync(returnUrl);
            if(context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if(!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }
            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if(context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if(client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if(client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = true,
                EnableLocalLogin = allowLocal,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel
            {
                LogoutId = logoutId,
                ShowLogoutPrompt = true
            };

            if(User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interactionService.GetLogoutContextAsync(logoutId);
            if(context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            var logout = await _interactionService.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = false,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            if(User?.Identity.IsAuthenticated == true)
            {
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if(idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
                    if(providerSupportsSignout)
                    {
                        if(vm.LogoutId == null)
                        {
                            vm.LogoutId = await _interactionService.CreateLogoutContextAsync();
                        }

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}
