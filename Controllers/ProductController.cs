using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using ShopCartUser.Models;
using ShopCartUser.Models.ViewModels;

namespace ShopCartUser.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IToastNotification _toastNotification;

        public ProductController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }
        //public IActionResult Index()
        //{
        //    var Product = ExecuteGetApi("Product/Get", 1);
        //    return View();
        //}
        public IActionResult Index(int prosuctid)
        {
            SingleProduct singleProduct = new SingleProduct();
            HttpCommonResponse ResData = ExecuteGetApi("Product/Get", prosuctid);
            if (ResData.success == true)
            {
                ProductMstr product = JsonConvert.DeserializeObject<ProductMstr>(JsonConvert.SerializeObject(ResData.data));
                singleProduct.Product = product;
                return View(singleProduct);
            }
            else
            {
                return this.RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public IActionResult addToCart(addTocart model)
        {
            try
            {
                
                if (Convert.ToInt32(Request.Cookies["UserId"]) > 0)
                {
                    object send = model;
                    HttpCommonResponse ResData = ExecutePostApi_Auth("Cart/Insert/"+model.subPropId, model);
                    if (ResData.statusCode == HttpStatusCode.Unauthorized)
                    {
                        
                        return RedirectToAction("Index", "Login", new { msg = "Plz Re-Login, Your Last Login is one day ago"});
                    }
                    if (ResData.success == true)
                    {
                        
                          
                        return this.RedirectToAction("Index", "Cart");
                    }
                    else
                    {
                        return this.RedirectToAction("Error", "Home");
                    }

                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
               

            }
            catch (Exception ex)
            {
                return this.RedirectToAction("Error", "Home");
            }


        }
    }
}