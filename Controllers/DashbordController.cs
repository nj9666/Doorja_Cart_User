using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopCartUser.Models;
using ShopCartUser.Models.ViewModels;

namespace ShopCartUser.Controllers
{
    public class DashbordController : BaseController
    {
        public IActionResult Index()
        {
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
    }
}