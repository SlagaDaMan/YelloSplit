using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YelloSplit.Helpers;

namespace YelloSplit.Controllers
{
    public class SiteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult about()
        {
            return View();
        }
        public IActionResult contact()
        {
            return View();
        }
        public IActionResult login()
        {
            return View();
        }
        public IActionResult registration()
        {
            return View();
        }
        public IActionResult faq()
        {
            return View();
        }
        public IActionResult features()
        {
            return View();
        }
        public IActionResult portfolio()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Authenticate(string Email, string Password)
        {
            UsersQueries Test = new UsersQueries();
            DataTable dt = new DataTable();
            dt = Test.ExecuteQueryFunction("Select * from Sys_Users");
            if (dt.Rows.Count > 0)
            {
                return RedirectToAction("Home","Index");
            }
            else
            {

            }
            return View();
        }

    }
}
