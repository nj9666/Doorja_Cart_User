using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopCartUser.Models;

namespace ShopCartUser.Controllers
{
    public class BaseController : Controller
    {

        private HttpClient ShopCartAPI = new HttpClient();
        public BaseController()
        {
            ShopCartAPI.BaseAddress = new Uri("https://localhost:44336/api/ShopAPI/");

            MediaTypeWithQualityHeaderValue ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            ShopCartAPI.DefaultRequestHeaders.Accept.Add(ContentType);
            HttpResponseMessage apiResponce = ShopCartAPI.GetAsync("User/Layout/CategoryAll").Result;
            string responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            HttpCommonResponse objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);

            List<CategoryMstr> CategoryMstr = JsonConvert.DeserializeObject<List<CategoryMstr>>(JsonConvert.SerializeObject(objlist.data));
            ViewBag.menu = CategoryMstr.ToList();
        }
    }
}