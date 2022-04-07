
using ShopOnline.Core.Models.Client;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopOnline.Business.Customer
{
    public interface IClientBusiness
    {
        Task InitBrands();
        Task<List<ProductInforViewModel>> GetProductsAsync(int? amountTake);
        Task<ProductDetailViewModel> GetDetailProductAsync(int id);
        Task<List<ProductInforModel>> GetCurrentProductsInforAsync(int amountTake);
        Task CreateReviewDetailAsync(ReviewDetailModel reviewDetail, ClaimsPrincipal user);
        Task<ProductsViewModel> GetProductsByBrandAsync(int brandId, int? typeId);
        Task<TypeOfBrandInforModel> GetTypesOfBrandAsync(int brandId);
    }
}
