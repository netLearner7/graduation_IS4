using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using is4.Models;
using is4.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace is4.Quickstart.Register
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEventService eventService;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IClientStore clientStore;
        private readonly LoginService loginService;

        public RegisterController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IEventService eventService, IIdentityServerInteractionService interaction, IClientStore clientStore,LoginService loginService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.eventService = eventService;
            this.interaction = interaction;
            this.clientStore = clientStore;
            this.loginService = loginService;
        }

        [HttpGet]
        public IActionResult Register(string returnURL)
        {
            ViewBag.returnURL = returnURL;
            return View();
        }

        [HttpPost]
        public  IActionResult Register(RegisterInputModel InputModel) {

            if (!ModelState.IsValid)
            {
                ViewBag.returnURL = InputModel.returnURL;
                return View();
            }

            var user= userManager.FindByNameAsync(InputModel.UserName).Result;

            //当用户已经存在的时候
            if (user != null) {                
                ModelState.AddModelError("", "用户已存在！");
                ViewBag.returnURL = InputModel.returnURL;
                return View();        
            }


            //创建用户部分
            user = new ApplicationUser()
            {
                UserName = InputModel.UserName
            };
            var createResult = userManager.CreateAsync(user, InputModel.PassWord).Result;

            //如果创建用户失败
            if (!createResult.Succeeded)
            {
                throw new Exception(createResult.Errors.First().Description);
            }

            //添加对应的claim
            createResult = userManager.AddClaimsAsync(user, new Claim[]{
                        new Claim(JwtClaimTypes.Email, InputModel.Email),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.PhoneNumber, InputModel.PhoneNumber)
                    }).Result;

            //如果添加失败
            if (!createResult.Succeeded)
            {
                throw new Exception(createResult.Errors.First().Description);
            }


            //登录函数
            var loginResult = loginService.Login(InputModel.returnURL, InputModel.UserName, InputModel.PassWord, user).Result;

            //登录成功则重定向
            if (loginResult.result)
            {
                return View("Redirect", new RedirectViewModel { RedirectUrl = InputModel.returnURL });
            }

            //登陆失败则显示错误信息
            ModelState.AddModelError("", loginResult.errMessage);
            ViewBag.returnURL = InputModel.returnURL;
            return View();

        }
    }
}