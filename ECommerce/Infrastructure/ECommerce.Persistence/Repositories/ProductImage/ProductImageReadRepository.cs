﻿using ECommerce.Application.Repositories;
using ECommerce.Application.Repositories.Abstracts;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{ 
    public class ProductImageReadRepository : ReadRepository<ProductImage>, IProductImageReadRepository
{
    public ProductImageReadRepository(ECommerceDbContext context) : base(context)
    {
    }
}
}
