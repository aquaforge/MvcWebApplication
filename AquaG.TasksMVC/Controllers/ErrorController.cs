using AquaG.TasksMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AquaG.TasksMVC.Controllers
{
    public class ErrorController : BaseController
    {

        [HttpGet]
        public IActionResult Index()
        {
            return Index(0);
        }


        [Route("Error/{id}")]
        [HttpGet]
        public IActionResult Index(int id)
        {
            ViewData["Title"] = "Произошла ошибка";
            ViewData["ErrorInfo"] = "";

            switch (id)
            {

                case 404:
                    ViewData["Title"] = "Страница не найдена";
                    if (HttpContext.Items.ContainsKey("originalPath"))
                        ViewData["ErrorInfo"] = HttpContext.Items["originalPath"] as string;
                    break;

                default:
                    break;
            }
            return View();
        }



    }
}
