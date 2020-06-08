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

            List<ProductMstr> productList = JsonConvert.DeserializeObject<List<ProductMstr>>(JsonConvert.SerializeObject(objlist.data));

            foreach (ProductMstr p in productList)
            {
                //apiResponce = ShopCartAPI.GetAsync("subProduct/GetAll/" + p.Id).Result;
                //responsedata = apiResponce.Content.ReadAsStringAsync().Result;
                //objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);

                List<SubProductTbl> subproductList = new List<SubProductTbl>();
                if (objlist.data != null)
                {
                    //subproductList = JsonConvert.DeserializeObject<List<SubProductTbl>>(JsonConvert.SerializeObject(objlist.data));
                    //subproductList = JsonConvert.DeserializeObject<List<SubProductTbl>>(JsonConvert.SerializeObject(p.SubProductTbl));
                    subproductList = p.SubProductTbl.ToList();
                }
                else
                {
                    SubProductTbl sp = new SubProductTbl();
                    sp.Price = 909090;
                    subproductList.Add(sp);
                }
                p.SubProductTbl = subproductList;
            }


            HomeVM homeVM = new HomeVM();
            homeVM.topCollection = productList;

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
    public class HttpCommonResponse
    {
        public string AuthToken { get; set; }
        public bool? success { get; set; }
        public bool isedit { get; set; }
        public string message { get; set; }
        public DateTime timestamp { get; set; }
        public dynamic data { get; set; }
        //TODO: update to Naming to metaData
        public MetaData metadata { get; set; }
    }

}
