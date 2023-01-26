using ECommerce.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().WithMessage("Lütfen ürün adını boş geçmeyiniz.");
            RuleFor(p => p.Name).MinimumLength(3).WithMessage("Lütfen ürün adını en az 3 karakterden oluşturunuz.");
            RuleFor(p => p.Name).MaximumLength(150).WithMessage("Lütfen ürün adını en fazla 150 karakterden oluşturunuz.");
            RuleFor(p => p.Stock).NotEmpty().NotNull().WithMessage("Lütfen stok bilgisini boş bırakmayınız");
            RuleFor(p => p.Stock).GreaterThanOrEqualTo(0);
            RuleFor(p => p.Price).NotEmpty().NotNull().WithMessage("Lütfen fiyat bilgisini boş bırakmayınız");
            RuleFor(p => p.Price).GreaterThanOrEqualTo(0);

        }
    }
}
