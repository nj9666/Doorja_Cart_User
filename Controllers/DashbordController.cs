using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShopCartUser.Controllers
{
    public class DashbordController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}