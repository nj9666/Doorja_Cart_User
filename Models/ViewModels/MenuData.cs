using Newtonsoft.Json;
using ShopCartUser.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShopCartUser.Models.ViewModels
{
    public class MenuData
    {
        static dynamic menu;

        static HttpClient ShopCartAPI = new HttpClient();
        public MenuData()
        {
            ShopCartAPI.BaseAddress = new Uri("https://localhost:44336/api/ShopAPI/");
        }
        public static void setmenu()
        {
            if (ShopCartAPI.BaseAddress == null)
            {
                ShopCartAPI.BaseAddress = new Uri("https://localhost:44336/api/ShopAPI/");
            }

            MediaTypeWithQualityHeaderValue ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            ShopCartAPI.DefaultRequestHeaders.Accept.Add(ContentType);
            HttpResponseMessage apiResponce = ShopCartAPI.GetAsync("User/Layout/CategoryAll").Result;
            string responsedata = apiResponce.Content.ReadAsStringAsync().Result;
            HttpCommonResponse objlist = JsonConvert.DeserializeObject<HttpCommonResponse>(responsedata);
            List<CategoryMstr> CategoryMstr = JsonConvert.DeserializeObject<List<CategoryMstr>>(JsonConvert.SerializeObject(objlist.data));

            menu = CategoryMstr.ToList();

            
        }
        public static dynamic getmenu()
        {
            return menu;
        }
    }
}
