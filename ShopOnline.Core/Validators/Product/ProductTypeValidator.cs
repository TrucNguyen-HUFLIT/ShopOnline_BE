using FluentValidation;
using ShopOnline.Core.Models.Product;

namespace ShopOnline.Core.Validators.Product
{
    public class ProductTypeValidator : AbstractValidator<ProductTypeInfor>
    {
        public ProductTypeValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
