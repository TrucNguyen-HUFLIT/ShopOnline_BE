using FluentValidation;
using ShopOnline.Core.Models.Product;

namespace ShopOnline.Core.Validators.Product
{
    public class BrandCreateValidator : AbstractValidator<BrandCreate>
    {
        public BrandCreateValidator()
        {
            RuleFor(x => x.BrandName).NotEmpty();
        }
    }
}
