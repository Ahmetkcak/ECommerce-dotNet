using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities
{
    public class InvoiceFile : File
    {
        public double Price { get; set; }
    }
}
