using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class Token
    {
        public String AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
