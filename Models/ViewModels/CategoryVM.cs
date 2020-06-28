using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCartUser.Models.ViewModels
{
    public class CategoryVM
    {
        public List<ProductMstr> CatProductList { get; set; }
        public List<ColoursTbl> CatcolorList { get; set; }

        public int catId { get; set; }
        public int colorIdFillter { get; set; }
        public int minPriceFillter { get; set; }
        public int maxPriceFillter { get; set; }

    }
}
