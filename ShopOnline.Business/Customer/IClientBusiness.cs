
using ShopOnline.Core.Models.Client;
using ShopOnline.Core.Models.Mobile;
using ShopOnline.Core.Validators.Paging;
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
        Task<IEnumerable<BrandInforModel>> GetBrandAsync();
        Task<IEnumerable<ProductDetailModel>> GetPopularProductsAsync();
        Task<IEnumerable<ProductDetailModel>> GetFavoriteProductsAsync();
        Task<ProductModel> GetProductByIdDetailAsync(int idProductDetail);
        Task<IEnumerable<ProductDetailModel>> GetProductsByIdBrandAsync(int idBrand);
        Task<IEnumerable<ProductDetailModel>> GetProductsByFilterAsync(string searchTerm);

        Task<ProductsViewModel> GetListProductOfBrandAsync(ProductParamsModel model);
        Task FavoriteProductAsync(int idProductDetail);
    }
}
