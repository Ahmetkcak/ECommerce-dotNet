using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.ProductImage.GetProductImages
{
    public class GetProductImagesQueryResponse
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
