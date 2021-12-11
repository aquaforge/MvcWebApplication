using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AquaG.TasksMVC.Models;
using AquaG.TasksMVC.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using AquaG.TasksDbModel;
using Microsoft.AspNetCore.Identity;

namespace AquaG.TasksMVC.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController() : base() { }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await DI_SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
            var model = new LoginModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await DI_SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError("", @"Неверные логин и\или пароль");
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Register(string returnUrl = "")
        {
            var model = new RegisterModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                User user = await DI_UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    user = new User { Email = model.Email, UserName = model.Email };
                    var result = await DI_UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        await DI_SignInManager.SignInAsync(user, model.RememberMe);
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    var result = await DI_SignInManager.PasswordSignInAsync(userName: model.Email, password: model.Password,
                        isPersistent: model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь существует");
                    }
                }
            }
            return View(model);
        }

        //private async Task Authenticate(User user)
        //{

        //    var claims = new List<Claim>
        //{
        //    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
        //    new Claim( "Id", user.Id.ToString()),
        //    new Claim( "UserGuid", user.UserGuid.ToString()),
        //    new Claim( "Caption", user.Caption)
        //};
        //    //string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
        //    //[Authorize(Roles = "admin")]
        //    //[Authorize(Policy ="paid")]

        //    ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        //}
    }
}