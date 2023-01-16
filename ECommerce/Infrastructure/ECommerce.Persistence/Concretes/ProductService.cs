using ECommerce.Application.Abstract;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Concretes
{
    public class ProductService : IProductService
    {
        public List<Product> getAll()
            =>new () 
            {
                new(){Id= 1,Name="Bardak",Price=100,Stock=12},
                new(){Id= 2,Name="Bardak 1",Price=200,Stock=100},
                new(){Id= 3,Name="Bardak 2",Price=220,Stock=9},
                new(){Id= 4,Name="Bardak 3",Price=155,Stock=15},
                new(){Id= 5,Name="Bardak 4",Price=3600,Stock=10},

            };
    }
}
