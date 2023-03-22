using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Order
{
    public class CompletedOrderDto
    {
        public string Emil { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserName { get; set; }
    }
}
