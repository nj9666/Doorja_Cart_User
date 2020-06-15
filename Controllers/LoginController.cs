using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using ShopCartUser.Models;
using ShopCartUser.Models.ViewModels;

namespace ShopCartUser.Controllers
{
    public class LoginController : BaseController
    {


        private readonly IToastNotification _toastNotification;

        public LoginController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        public IActionResult Index(string msg)
        {
            if (msg != null)
            {
                _toastNotification.AddWarningToastMessage(msg);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login_frm(LoginViewModel model)
        {
            try
            {
                object send = model;
                HttpCommonResponse ResData = ExecutePostApi("User/Login", model);

                if (ResData.success == true)
                {
                    UserMstr user = JsonConvert.DeserializeObject<UserMstr>(JsonConvert.SerializeObject(ResData.data));
                    if (user != null)
                    {
                        HttpContext.Session.SetObject("User", user);
                        HttpContext.Session.SetInt32("UserId", user.Id);
                        CookieOptions userCookie = new CookieOptions();
                        userCookie.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Append("Token", ResData.AuthToken, userCookie);
                        Response.Cookies.Append("UserId", user.Id.ToString(), userCookie);
                    }
                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    return this.RedirectToAction("Error", "Home");
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index","Login");
            }


        }




        public IActionResult Registration()
        {
            return View();
        }
    }
}