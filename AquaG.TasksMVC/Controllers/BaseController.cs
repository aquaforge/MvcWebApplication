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

namespace AquaG.TasksMVC.Controllers
{
    public class BaseController : Controller
    {
        private TasksDbContext _db;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private ILogger<BaseController> _logger;

        public TasksDbContext DI_Db
        {
            get
            {
                if (_db == null) _db = HttpContext.RequestServices.GetRequiredService<TasksDbContext>();
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

        }

        public override RedirectResult Redirect(string url)
        {
            // Редирект только внутри сайта
            url = (!string.IsNullOrEmpty(url) && Url.IsLocalUrl(url)) ? url : "/";
            return base.Redirect(url);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}