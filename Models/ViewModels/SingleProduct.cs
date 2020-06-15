using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCartUser.Models.ViewModels
{
    public class SingleProduct
    {
        public ProductMstr Product { get; set; }
        public int userId { get; set; }
        public int subPropId { get; set; }
        public int quantity { get; set; }
    }
    public class addTocart
    {
        public int userId { get; set; }
        public int subPropId { get; set; }
        public int quantity { get; set; }
    }
}
