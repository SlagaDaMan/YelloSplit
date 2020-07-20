using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            dt = Test.ExecuteQueryFunction("Select * from App_Users Where EmailAddress = '" + Email + "' AND Password = '"  + Password + "'");
            var ID = "";


            if (dt.Rows.Count > 0)
            {
              
                ID = dt.Rows[0][0].ToString();
                //var IDe = Request.Cookies["ID"].ToString();

                CookieOptions cookies = new CookieOptions();
                cookies.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Append("ID", ID, cookies);
                return RedirectToAction("MainMenu", "Home");
            }
            else
            {
                return RedirectToAction("login");
            }
         
        }

        public IActionResult Reg(string FirstName, string LastName, string EmailAddress, string PhoneNo, string Password, string RetypePassword, string ApplicationType)
        {
            UsersQueries ex = new UsersQueries();
            DataTable dt = new DataTable();
         
            dt = ex.ExecuteQueryFunction("Select * from App_Users Where EmailAddress = '" + EmailAddress + "'");

            if (dt.Rows.Count > 0)
            {

                return RedirectToAction("registration");


            }
            else
            {
                var CreatedDate = DateTime.Now.ToString();
                DataTable apptype = ex.ExecuteQueryFunction($"Select ID from Lk_ApplicationTypes Where Description = '{ApplicationType}'");
                ex.ExecuteQueryFunction($"Insert into App_Users(ApplicationTypeID,FirstName,LastName,EmailAddress,Password,Tel,Credits,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) Values ( " +
                                                                $"'{apptype.Rows[0][0].ToString()}','{ FirstName}','{LastName}','{EmailAddress}','{Password}','{PhoneNo}',{30},'{"APP"}','{CreatedDate}','{"APP"}','{CreatedDate}')");
                return RedirectToAction("Welcome");
            }
            
            

        }

        public IActionResult Welcome()
        {
            return View();
        }

    }
}
