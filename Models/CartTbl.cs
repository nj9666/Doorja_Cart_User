﻿using System;
using System.Collections.Generic;

namespace ShopCartUser.Models
{
    public partial class CartTbl
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SubProducatId { get; set; }
        public decimal Qty { get; set; }
        public DateTime CreateDt { get; set; }
        public DateTime UpdateDt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public SubProductTbl SubProducat { get; set; }
        public UserMstr User { get; set; }
    }
}
