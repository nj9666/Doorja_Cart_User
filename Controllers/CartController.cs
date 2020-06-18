using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopCartUser.Models;
using ShopCartUser.Models.ViewModels;

namespace ShopCartUser.Controllers
{
    public class CartController : BaseController
    {
        public IActionResult Index()
        {
            CartView cart = new CartView();
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {
                HttpCommonResponse ResData = ExecuteGetApi_Auth("Cart/GetAll", null);
                if (ResData.statusCode == HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Index", "Login", new { msg = "Plz Re-Login, Your Last Login is one day ago" });
                }
                if (ResData.success == true)
                {
                    cart.CartList = JsonConvert.DeserializeObject<List<CartTbl>>(JsonConvert.SerializeObject(ResData.data));

                    return View(cart);
                }
                else
                {
                    return this.RedirectToAction("Error", "Home");
                }

            }
            else
            {
                return RedirectToAction("Index", "Login", new { msg = "Plz Login First" }); ;
            }
        }
        [HttpPost]
        public String chanesQty(int id, int qty)
        {
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {

                HttpCommonResponse ResData = ExecutePostApi_Auth("Cart/Qty/" + id + "/" + qty, null);
                if (ResData.statusCode == HttpStatusCode.Unauthorized)
                {
                    var msg = "Plz Re-Login, Your Last Login is one day ago";
                    return msg;
                }
                if (ResData.success == true)
                {
                    var msg = "Qty Change sucessfully";
                    return msg;
                }
                else
                {

                    var msg = "Qty Change NOt done plz Refresh this page and try again";
                    return msg;
                }
                

            }
            else
            {
                var msg = "You Need to login first";
                return msg;
            }
        }

        [HttpPost]
        public String RemoveCart(int id)
        {
            if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
            {

                HttpCommonResponse ResData = ExecutePostApi_Auth("Cart/Qty/" + id, null);
                if (ResData.statusCode == HttpStatusCode.Unauthorized)
                {
                    var msg = "Plz Re-Login, Your Last Login is one day ago";
                    return msg;
                }
                if (ResData.success == true)
                {
                    var msg = "Qty Change sucessfully";
                    return msg;
                }
                else
                {

                    var msg = "Qty Change NOt done plz Refresh this page and try again";
                    return msg;
                }


            }
            else
            {
                var msg = "You Need to login first";
                return msg;
            }
        }
    }
}