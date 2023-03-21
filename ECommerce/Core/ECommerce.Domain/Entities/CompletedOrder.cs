using ECommerce.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class CompletedOrder : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
