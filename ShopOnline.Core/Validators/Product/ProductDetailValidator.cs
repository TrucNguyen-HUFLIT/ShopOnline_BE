using FluentValidation;
using ShopOnline.Core.Models.Product;

namespace ShopOnline.Core.Validators.Product
{
    public class ProductDetailValidator : AbstractValidator<ProductDetailInfor>
    {
        public ProductDetailValidator()
        {
        }
    }
    public class ProductDetailCreateValidator : AbstractValidator<ProductDetailCreate>
    {
        public ProductDetailCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.BasePrice).NotEmpty();
            RuleFor(x => x.UploadPic1).NotEmpty();
        }
    }
    public class ProductDetailUpdateValidator : AbstractValidator<ProductDetailUpdate>
    {
        public ProductDetailUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.BasePrice).NotEmpty();
            //RuleFor(x => x.UploadPic1).NotEmpty();
        }
    }
}
