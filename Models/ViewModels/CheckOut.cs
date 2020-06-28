using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCartUser.Models.ViewModels
{
    public class CheckOut
    {
        public UserMstr userdt { get; set; }
        public List<CartTbl> FinalProductList { get; set; }
        public List<AddressTbl> useraddresses { get; set; }
        public int addId { get; set; }
        public string cpCode { get; set; }
    }
    public class PayResponse
    {
        public string TXNID { get; set; }
        public string ORDERID { get; set; }
        public string STATUS { get; set; }
        public string TXNAMOUNT { get; set; }
        public string RESPMSG { get; set; }
        public string PAYMENTMODE { get; set; }

        public OrderTbl order { get; set; }
        public UserMstr userdt { get; set; }
    }
}
