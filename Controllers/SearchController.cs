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
    public class SearchController : BaseController
    {
        public IActionResult Index(string searchText)
        {
            SearchModel searchRes = new SearchModel();


            if (searchText != null && searchText != "" && searchText != " ")
            {
                HttpCommonResponse ResData = ExecuteGetApi("Product/Search/" + searchText);
                if (ResData.success == true)
                {
                    searchRes.SearchProducts = JsonConvert.DeserializeObject<List<ProductMstr>>(JsonConvert.SerializeObject(ResData.data));
                }
                else
                {
                    ViewBag.nodata = ResData.message;
                }
                return View(searchRes);
            }
            return this.RedirectToAction("Index", "Home", new { msg = "Enter Valid Keyword for search !!" });

        }
    }
}