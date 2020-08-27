using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCartUser.Models.ViewModels
{
    public class SearchModel
    {
        public string SearchText { get; set; }
        public List<ProductMstr> SearchProducts { get; set; }
    }
}
