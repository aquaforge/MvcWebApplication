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
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AquaG.TasksMVC.Controllers
{
    public class BaseController : Controller
    {
        protected TasksDbContext _db;
        protected UserManager<User> _userManager;
        protected SignInManager<User> _signInManager;
        protected ILogger<BaseController> _logger;

        protected User _authorizedUser;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _db = HttpContext.RequestServices.GetRequiredService<TasksDbContext>();
            _userManager = HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
            _signInManager = HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
            _logger = HttpContext.RequestServices.GetRequiredService<ILogger<BaseController>>();

            if (User.Identity.IsAuthenticated)
                _authorizedUser = _userManager.FindByEmailAsync(User.Identity.Name).GetAwaiter().GetResult();



            base.OnActionExecuting(context);
        }








        public TasksDbContext DI_Db
        {
            get
            {
                return _db;
            }
        }

        public UserManager<User> DI_UserManager
        {
            get
            {
                if (_userManager == null) _userManager = HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                return _userManager;
            }
        }
        public SignInManager<User> DI_SignInManager
        {
            get
            {
                if (_signInManager == null) _signInManager = HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                return _signInManager;
            }
        }
        public ILogger<BaseController> DI_Logger
        {
            get
            {
                if (_logger == null) _logger = HttpContext.RequestServices.GetRequiredService<ILogger<BaseController>>();
                return _logger;
            }
        }



        public BaseController()
        {
            //Console.WriteLine(HttpContext.Request.Path);
        }

        public override RedirectResult Redirect(string url)
        {
            // Редирект только внутри сайта
            url = (!string.IsNullOrEmpty(url) && Url.IsLocalUrl(url)) ? url : "/";
            return base.Redirect(url);
        }


    }
}