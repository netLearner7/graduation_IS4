using IdentityServer4.Events;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using is4.Models;
using is4.Quickstart.Register;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace is4.Service
{
    public class LoginService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEventService eventService;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IClientStore clientStore;

        public LoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEventService eventService, IIdentityServerInteractionService interaction, IClientStore clientStore)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.eventService = eventService;
            this.interaction = interaction;
            this.clientStore = clientStore;
        }

        public async Task<LoginResult> Login(string returnURL,string UserName,string  PassWord,ApplicationUser user) {

            var context = await interaction.GetAuthorizationContextAsync(returnURL);

            var loginResult = await signInManager.PasswordSignInAsync(UserName, PassWord, false, true);

            if (loginResult.Succeeded)
            {

                user = await userManager.FindByNameAsync(UserName);
                
                //发布登录成功事件
                await eventService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.ClientId));

                if (context != null)
                {
                        return new LoginResult() { result = true };
                }

                return new LoginResult() { result = false, errMessage = "非客户端请求" };
            }

            //发布登录失败事件
            await eventService.RaiseAsync(new UserLoginFailureEvent(user.UserName, "invalid credentials", clientId: context?.ClientId));

            return new LoginResult() { result = false, errMessage = "账号密码错误" };

        }

        
    }

    public class LoginResult {
        public bool result { get; set; }
        public string errMessage { get; set; }
    }
}
