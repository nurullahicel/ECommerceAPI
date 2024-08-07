﻿using ECommerceAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        //public Guid CustomerId { get; set; }        
     
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Basket Basket { get; set; }
        //public ICollection<Product>? Products { get; set; }
        //public Customer? Customer { get; set; }
    }
}
