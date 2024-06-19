using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PI = ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.Features.Queries.Product.GetByIdProduct
{
    public class GetByIdProductQueryResponse
    {
        public string Name { get; set; } = string.Empty;
        public int Stock { get; set; }
        public float Price { get; set; }
       
    }
}
