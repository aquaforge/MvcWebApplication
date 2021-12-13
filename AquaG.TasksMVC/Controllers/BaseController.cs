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

        protected delegate TOutput GetViewModelFromOneRecord<TOutput, TSource>(TSource val);

        protected async Task<IActionResult> GetOneRecordFromDbAsActionResult<TSource, TOutput>(
            System.Linq.IQueryable<TSource> source,
            System.Linq.Expressions.Expression<Func<TSource, bool>> predicate,
            GetViewModelFromOneRecord<TOutput, TSource> func,
            string viewName = null)
        {
            if (_authorizedUser == null) return Unauthorized();

            TSource t = await source.FirstOrDefaultAsync(predicate);
            if (t == null) return BadRequest();

            if (viewName == null)
                return View(func.Invoke(t));
            else
                return View(viewName, func.Invoke(t));
        }

    

    //protected async Task<TOutput> GetOneRecordFromDb<TSource, TOutput>(
    //    System.Linq.IQueryable<TSource> source,
    //    System.Linq.Expressions.Expression<Func<TSource, bool>> predicate,
    //    GetViewModelFromOneRecord<TOutput, TSource> func)
    //{
    //    if (_authorizedUser == null) throw new ArgumentException("GetOneRecordFromDb");

    //    TSource t = await source.FirstOrDefaultAsync(predicate);
    //    if (t == null) throw new ArgumentException("GetOneRecordFromDb");  

    //    return func.Invoke(t);
    //}



    [NonAction]
    public override RedirectResult Redirect(string url)
    {
        // Редирект только внутри сайта
        url = (!string.IsNullOrEmpty(url) && Url.IsLocalUrl(url)) ? url : "/";
        return base.Redirect(url);
    }


}
}