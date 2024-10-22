﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCartUser.Models.ViewModels
{
    public class ProfileDeshbord
    {
        public UserMstr loginUser { get; set; }
        public List<AddressTbl> useraddresses { get; set; }
        public AddressTbl userDefultaddresses { get; set; }
        public int addId { get; set; }
        public List<CoutOrderTbl> MyOrders { get; set; }

        [Required]
        public string oldpass { get; set; }
        [Required]
        public string newpass { get; set; }
        [Required]
        [Compare("newpass", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string cnewpass { get; set; }
    }
    public class CoutOrderTbl
    {
        public int Id { get; set; }
        public String OrderIdV { get; set; }
        public decimal TotalQty { get; set; }
        public decimal TotalPrice { get; set; }
        public byte Status { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreateDt { get; set; }
        public string CouponCode { get; set; }
        public ICollection<string> ProductImg { get; set; }
    }
    public class orderDetails
    {
        public int Id { get; set; }
        public String OrderIdV { get; set; }

        public decimal TotalQty { get; set; }
        public decimal TotalPrice { get; set; }
        public byte Status { get; set; }
        public int PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreateDt { get; set; }
        public string CouponCode { get; set; }

        public UserMstr User { get; set; }
        public AddressTbl orderaddresses { get; set; }
        public List<OrderDetailsTbl> MyOrders { get; set; }

    }
    public class changePass
    {
        [Required]
        public string oldpass{ get; set; }
        [Required]
        public string newpass{ get; set; }
        [Required]
        [Compare("newpass", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string cnewpass{ get; set; }
    }
}
