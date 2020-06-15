using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopCartUser.Models;
using ShopCartUser.Models.ViewModels;

namespace ShopCartUser.Controllers
{
    public class HomeController : BaseController
    {

        private HttpClient ShopCartAPI = new HttpClient();
        public HomeController()
        {
            ShopCartAPI.BaseAddress = new Uri("https://localhost:44336/api/ShopAPI/");
        }
        public IActionResult Index()
        {
            MediaTypeWithQualityHeaderValue ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            ShopCartAPI.DefaultRequestHeaders.Accept.Add(ContentType);
            HttpResponseMessage apiResponce = ShopCartAPI.GetAsync("User/Home/TopColl").Result;
            string responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            HttpCommonResponse objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);

            List<ProductMstr> topCollection = new List<ProductMstr>();
            if (objlist.success == true)
            {
                foreach (ProductMstr topcPro in JsonConvert.DeserializeObject<List<ProductMstr>>(JsonConvert.SerializeObject(objlist.data)))
                {
                    topCollection.Add(topcPro);
                }
            }

            apiResponce = ShopCartAPI.GetAsync("User/Home/TodayDeal").Result;
            responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);

            List<ProductMstr> todayOffer = new List<ProductMstr>();
            if (objlist.success == true)
            {
                foreach (ProductMstr tdOffPro in JsonConvert.DeserializeObject<List<ProductMstr>>(JsonConvert.SerializeObject(objlist.data)))
                {
                    todayOffer.Add(tdOffPro);
                }
            }

            apiResponce = ShopCartAPI.GetAsync("User/Home/NewProduct").Result;
            responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);

            List<ProductMstr> NewProduct = new List<ProductMstr>();
            if (objlist.success == true)
            {
                foreach (ProductMstr tdOffPro in JsonConvert.DeserializeObject<List<ProductMstr>>(JsonConvert.SerializeObject(objlist.data)))
                {
                    NewProduct.Add(tdOffPro);
                }
            }

            apiResponce = ShopCartAPI.GetAsync("User/Home/BestSellProduct").Result;
            responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);

            List<ProductMstr> BestSeller = new List<ProductMstr>();
            if (objlist.success == true)
            {
                foreach (ProductMstr tdOffPro in JsonConvert.DeserializeObject<List<ProductMstr>>(JsonConvert.SerializeObject(objlist.data)))
                {
                    BestSeller.Add(tdOffPro);
                }
            }

            UserMstr user = HttpContext.Session.GetObject<UserMstr>("User");

            HomeVM homeVM = new HomeVM();
            homeVM.TopCollection = topCollection;
            homeVM.TodayOffer = todayOffer;
            homeVM.NewProduct = NewProduct;
            homeVM.BestSeller = BestSeller;
            return View(homeVM);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult logout()
        {
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("Token");
            HttpContext.Session.Clear();
            return this.RedirectToAction("Index", "Home");
        }
    }
    public class InputData
    {
        public bool isedit { get; set; }
        public string AuthToken { get; set; }
        public string status { get; set; }
        public bool? success { get; set; }
        public string message { get; set; }
        public DateTime timestamp { get; set; }
        public dynamic Data { get; set; }
        public string id { get; set; }
        public string ProductId { get; set; }
    }
    public class MetaData
    {
        public int? totalCount { get; set; }
        public int? start { get; set; }
        public int? count { get; set; }
    }
}
