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

namespace AquaG.TasksMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly TasksDbContext db;

        public AccountController(TasksDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);

                    return Redirect(GetLocalRedirectString(model.ReturnUrl));
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
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
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    user = new User { Email = model.Email, Password = model.Password, Caption = model.Caption };
                    db.Users.Add(user);
                    await db.SaveChangesAsync();

                    await Authenticate(user);
                    return Redirect(GetLocalRedirectString(model.ReturnUrl));
                }
                else
                {
                    if (user.Password == model.Password)
                    {
                        if (user.Caption != model.Caption)
                        {
                            user.Caption = model.Caption;
                            await db.SaveChangesAsync();
                        }

                        await Authenticate(user);
                        return Redirect(GetLocalRedirectString(model.ReturnUrl));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Пользователь уже существует");
                    }
                }

            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim( "Id", user.Id.ToString()),
                new Claim( "UserGuid", user.UserGuid.ToString()),
                new Claim( "Caption", user.Caption)
            };
            //string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            //[Authorize(Roles = "admin")]
            //[Authorize(Policy ="OnlyForLondon")]

            ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        private string GetLocalRedirectString(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return returnUrl;
            else
                return "/";
        }

    }
}