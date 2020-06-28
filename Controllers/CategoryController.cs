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
    public class CategoryController : BaseController
    {
        public IActionResult Index(int id)
        {
            CategoryVM category = new CategoryVM();
            HttpCommonResponse ResData = ExecuteGetApi_Auth("User/Category/" + id, null);

            if (ResData.success == true)
            {
                category.CatProductList = JsonConvert.DeserializeObject<List<ProductMstr>>(JsonConvert.SerializeObject(ResData.data));
                category.CatcolorList = new List<ColoursTbl>();
                foreach (ProductMstr product in category.CatProductList)
                {
                    foreach (SubProductTbl spro in product.SubProductTbl)
                    {
                        if (!category.CatcolorList.Where(p => p.Id == spro.ColorId).Any())
                        {
                            category.CatcolorList.Add(spro.Color);
                        }
                    }
                }
            }
            else
            {
                category.CatProductList = new List<ProductMstr>();
            }
            category.catId = id;
            return View(category);
        }
        [HttpPost]
        public IActionResult chanesfillter(CategoryVM model)
        {
            CategoryVM category = new CategoryVM();
            HttpCommonResponse ResData = ExecuteGetApi_Auth("User/Category/" + model.catId, null);
            List<ProductMstr> fillterList = new List<ProductMstr>();
            bool isaddble = false;

            if (ResData.success == true)
            {
                category.CatProductList = JsonConvert.DeserializeObject<List<ProductMstr>>(JsonConvert.SerializeObject(ResData.data));
                category.CatcolorList = new List<ColoursTbl>();
                foreach (ProductMstr product in category.CatProductList)
                {
                    isaddble = false;
                    foreach (SubProductTbl spro in product.SubProductTbl)
                    {
                        if (!category.CatcolorList.Where(p => p.Id == spro.ColorId).Any())
                        {
                            category.CatcolorList.Add(spro.Color);
                        }


                        if (model.colorIdFillter != 0 && (model.minPriceFillter != 0 || model.maxPriceFillter != 0))
                        {
                            if (model.maxPriceFillter != -1)
                            {
                                if (model.colorIdFillter == spro.ColorId && model.minPriceFillter <= spro.Price && model.maxPriceFillter >= spro.Price)
                                {
                                    isaddble = true;
                                }
                            }
                            else
                            {
                                if (model.colorIdFillter == spro.ColorId && model.minPriceFillter <= spro.Price)
                                {
                                    isaddble = true;
                                }
                            }
                        }
                        else if (model.colorIdFillter != 0 && model.colorIdFillter == spro.ColorId)
                        {
                            isaddble = true;
                        }
                        else if (model.minPriceFillter != 0 || model.maxPriceFillter != 0)
                        {
                            if (model.maxPriceFillter != -1)
                            {
                                if (model.minPriceFillter <= spro.Price && model.maxPriceFillter >= spro.Price)
                                {
                                    isaddble = true;
                                }
                            }
                            else
                            {
                                if (model.minPriceFillter >= spro.Price)
                                {
                                    isaddble = true;
                                }
                            }
                        }
                    }
                    if (isaddble)
                    {
                        fillterList.Add(product);
                    }
                }

                category.CatProductList = fillterList;
            }
            else
            {
                category.CatProductList = new List<ProductMstr>();
            }
            category.colorIdFillter = model.colorIdFillter;
            category.minPriceFillter = model.minPriceFillter;
            category.maxPriceFillter = model.maxPriceFillter;
            category.catId = model.catId;
            return View("Index", category);
        }
    }
}