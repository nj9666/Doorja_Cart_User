using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using ShopCartUser.Models;
using ShopCartUser.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace ShopCartUser.Controllers
{
    public class DashbordController : BaseController
    {

        private readonly IToastNotification _toastNotification;

        public DashbordController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        public IActionResult Index(string msg)
        {
            if (msg != null)
            {
                _toastNotification.AddWarningToastMessage(msg);
            }
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {


                ProfileDeshbord PDObj = new ProfileDeshbord();
                PDObj.loginUser = JsonConvert.DeserializeObject<UserMstr>(Request.Cookies["User"]);

                HttpCommonResponse ResData1 = ExecuteGetApi_Auth("Address/GetAll/My", null);
                if (ResData1.success == true)
                {
                    PDObj.useraddresses = JsonConvert.DeserializeObject<List<AddressTbl>>(JsonConvert.SerializeObject(ResData1.data));
                    foreach (AddressTbl addItem in PDObj.useraddresses)
                    {
                        if (addItem.IsDefault)
                        {
                            PDObj.userDefultaddresses = addItem;
                        }
                    }
                }
                else
                {
                    PDObj.useraddresses = new List<AddressTbl>();
                }

                HttpCommonResponse ResData2 = ExecuteGetApi_Auth("Order/GetAll/ForUser", null);
                if (ResData2.success == true)
                {
                    PDObj.MyOrders = JsonConvert.DeserializeObject<List<CoutOrderTbl>>(JsonConvert.SerializeObject(ResData2.data));

                }
                else
                {
                    PDObj.MyOrders = new List<CoutOrderTbl>();
                }

                return View(PDObj);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { msg = "Plz Login First" });
            }
        }
        public IActionResult OrderDitails(int orderid)
        {
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                orderDetails ODObj = new orderDetails();

                HttpCommonResponse ResData1 = ExecuteGetApi_Auth("Order/GetForOD/" + orderid, null);
                if (ResData1.success == true)
                {
                    ODObj = JsonConvert.DeserializeObject<orderDetails>(JsonConvert.SerializeObject(ResData1.data));
                }
                else
                {
                    
                }

                return View(ODObj);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { msg = "Plz Login First" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult change_pwd(changePass threepass)
        {
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                try
                {
                    HttpCommonResponse ResData = ExecutePostApi_Auth("User/Changepwd", threepass);

                    if (ResData.success == true)
                    {
                        return this.RedirectToAction("Index", "Dashbord", new { msg = ResData.message });
                    }
                    else
                    {
                        return this.RedirectToAction("Index", "Dashbord", new { msg = ResData.message });
                    }

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { msg = "Plz Login First" });
            }
        }

        public IActionResult newaddress(string msg)
        {
            if (msg != null)
            {
                _toastNotification.AddWarningToastMessage(msg);
            }
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                List<CityMstr> citys = new List<CityMstr>();
                HttpCommonResponse Rescitys = ExecuteGetApi("City/GetAll");
                if (Rescitys.success == true)
                {
                    citys = JsonConvert.DeserializeObject<List<CityMstr>>(JsonConvert.SerializeObject(Rescitys.data));

                }
                List<StateMstr> states = new List<StateMstr>();
                HttpCommonResponse Resstates = ExecuteGetApi("State/GetAll");
                if (Resstates.success == true)
                {
                    states = JsonConvert.DeserializeObject<List<StateMstr>>(JsonConvert.SerializeObject(Resstates.data));

                }
                List<CountryMstr> countrys = new List<CountryMstr>();
                HttpCommonResponse Rescountrys = ExecuteGetApi("Country/GetAll");
                if (Rescountrys.success == true)
                {
                    countrys = JsonConvert.DeserializeObject<List<CountryMstr>>(JsonConvert.SerializeObject(Rescountrys.data));

                }



            ViewBag.citys = citys;
            ViewBag.states = states;
            ViewBag.countrys = countrys;
            return View();

            }
            return this.RedirectToAction("Index", "Login", new { msg = "Please Login First !!" });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addAaddress(AddressTbl address)
        {
            if (!ModelState.IsValid)
            {
                return View("newaddress");
            }
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                try
                {
                    HttpCommonResponse ResData = ExecutePostApi_Auth("Address/Insert", address);

                    if (ResData.success == true)
                    {                        
                        return this.RedirectToAction("Index", "Dashbord", new { msg = ResData.message });
                    }
                    else
                    {
                        return this.RedirectToAction("Index", "Dashbord", new { msg = ResData.message });
                    }

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Login", new { msg = "Something Wrong Please try again !" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { msg = "Plz Login First" });
            }
        }
        [HttpPost]
        public string setdefultAdd(int addId)
        {
            
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                try
                {
                    HttpCommonResponse ResData = ExecutePostApi_Auth("Address/Set/Default/"+addId, null);
                    if (ResData.success == true)
                    {
                        return ResData.message;
                    }
                    else
                    {
                        return ResData.message;
                    }

                }
                catch (Exception ex)
                {
                    return "Something Wrong Please try again !";
                }
            }
            else
            {
                return "Please Login First";
            }
        }
        [HttpPost]
        public string removeAddress(int addId)
        {
            
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                try
                {
                    HttpCommonResponse ResData = ExecutePostApi_Auth("Address/Remove/" + addId, null);
                    if (ResData.success == true)
                    {
                        return ResData.message;
                    }
                    else
                    {
                        return ResData.message;
                    }

                }
                catch (Exception ex)
                {
                    return "Something Wrong Please try again !";
                }
            }
            else
            {
                return "Please Login First";
            }
        }


        public IActionResult profile(string msg)
        {
            if (msg != null)
            {
                _toastNotification.AddWarningToastMessage(msg);
            }
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                UserMstr user = new UserMstr();
                HttpCommonResponse Resstates = ExecuteGetApi_Auth("User/Get",null);
                if (Resstates.success == true)
                {
                    user = JsonConvert.DeserializeObject<UserMstr>(JsonConvert.SerializeObject(Resstates.data));

                }

                return View(user);

            }
            return this.RedirectToAction("Index", "Login", new { msg = "Please Login First !!" });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult saveprofile(UserMstr user)
        {
            if (!ModelState.IsValid)
            {
                return View("profile");
            }
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                try
                {
                    int uid = Convert.ToInt32(Request.Cookies["UserId"]);
                    HttpCommonResponse ResData = ExecutePostApi_Auth("User/Edit/"+ uid, user);
                    if (ResData.success == true)
                    {
                        CookieOptions userCookie = new CookieOptions();
                        Response.Cookies.Append("User", JsonConvert.SerializeObject(user), userCookie);
                        return this.RedirectToAction("Index", "Dashbord", new { msg = ResData.message });
                    }
                    else
                    {
                        return this.RedirectToAction("profile", "Dashbord", new { msg = ResData.message });
                    }

                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Login", new { msg = "Something Wrong Please try again !" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { msg = "Plz Login First" });
            }
        }


    }
}