using ShopCartUser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCartUser.Models.ViewModels
{
    public class HomeVM
    {
        public List<ProductMstr> TopCollection { get; set; }
        public List<ProductMstr> NewProduct { get; set; }
        public List<ProductMstr> BestSeller { get; set; }
        public List<ProductMstr> TodayOffer { get; set; }
    }
}
