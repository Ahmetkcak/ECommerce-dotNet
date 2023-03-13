using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Order
{
    public class CreateOrder
    {
        public int? BasketId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
    }
}
