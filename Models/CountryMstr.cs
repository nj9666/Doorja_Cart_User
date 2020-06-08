﻿using System;
using System.Collections.Generic;

namespace ShopCartUser.Models
{
    public partial class CountryMstr
    {
        public CountryMstr()
        {
            AddressTbl = new HashSet<AddressTbl>();
            VenderMstr = new HashSet<VenderMstr>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDt { get; set; }
        public DateTime UpdateDt { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<AddressTbl> AddressTbl { get; set; }
        public ICollection<VenderMstr> VenderMstr { get; set; }
    }
}
