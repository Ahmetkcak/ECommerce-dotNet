using ECommerce.Domain.Entities.Common;
using ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class Basket : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Order Order { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}
