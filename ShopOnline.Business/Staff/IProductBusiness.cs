using ShopOnline.Core.Models.Product;
using ShopOnline.Core.Validators.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace ShopOnline.Business.Staff
{
    public interface IProductBusiness
    {
        // Product Brand 
        Task<IPagedList<BrandInfor>> GetListBrandAsync(string sortOrder, string currentFilter, string searchString, int? page);
        Task CreateBrandAsync(BrandCreate brandCreate);
        Task<bool> EditBrandAsync(BrandInfor brandInfor);
        Task<BrandInfor> GetBrandByIdAsync(int id);
        Task<bool> DeleteBrandAsync(int id);

        // Product Type
        Task<IPagedList<ProductTypeInfor>> GetListProductTypeAsync(string sortOrder, string currentFilter, string searchString, int? page);
        ProductTypeInfor GetProductTypeByIdAsync(int id);
        Task<bool> EditProductTypeAsync(ProductTypeInfor productType);
        Task<List<BrandInfor>> GetListBrand();
        Task CreateProductTypeAsync(ProductTypeInfor productTypeInfor);
        Task<bool> DeleteProductTypeAsync(int id);


        // ProductDetail
        Task<IPagedList<ProductDetailInfor>> GetListProductDetailAsync(string sortOrder, string currentFilter, string searchString, int? page);
        Task<List<ProductTypeInfor>> GetListProductType();
        Task<ProductDetailUpdate> GetProductDetailByIdAsync(int id);
        Task<int> CreateProductDetailAsync(ProductDetailCreate productDetailCreate);
        Task<bool> UpdateProductDetailAsync(ProductDetailUpdate productDetailUpdate);
        Task<bool> DeleteProductDetailAsync(int id);
        Task<PagedCollectionResultModel<ProductDetailInfor>> GetListProductDetailsAsync(ProductDetailParamsModel model);


        // Product
        Task<IPagedList<ProductInfor>> GetListProductAsync(string sortOrder, string currentFilter, string searchString, int? page);
        Task<PagedCollectionResultModel<ProductInfor>> GetListProductAsync(ProductParamsModel model);
        Task<List<ProductDetailInfor>> GetListProductDetail();
        Task<ProductUpdate> GetProductByIdAsync(int id);
        Task<int> CreateProductAsync(ProductCreate productCreate);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> UpdateProductAsync(ProductUpdate productUpdate);

    }
}
