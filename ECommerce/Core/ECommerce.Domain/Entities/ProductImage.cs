﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class ProductImage : File
    {
        public bool Showcase { get; set; }     
        public ICollection<Product> Products { get; set; }
    }
}
