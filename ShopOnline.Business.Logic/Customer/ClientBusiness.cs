using Microsoft.EntityFrameworkCore;
using ShopOnline.Business.Customer;
using ShopOnline.Core;
using ShopOnline.Core.Entities;
using ShopOnline.Core.Exceptions;
using ShopOnline.Core.Helpers;
using ShopOnline.Core.Models.Client;
using ShopOnline.Core.Models.Enum;
using ShopOnline.Core.Models.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;
using ProductDetailModel = ShopOnline.Core.Models.Mobile.ProductDetailModel;
using ProductInforModel = ShopOnline.Core.Models.Client.ProductInforModel;

namespace ShopOnline.Business.Logic.Customer
{
    public class ClientBusiness : IClientBusiness
    {
        private readonly MyDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ClientBusiness(MyDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<ProductInforViewModel>> GetProductsAsync(int? amountTake)
        {
            var productsInfor = new List<ProductInforViewModel>();
            var currentBrandIds = BrandSingleton.Instance.BrandInfors.Select(x => x.Id).ToList();

            var productsDetail = await _context.ProductDetails
                                        .Where(x => currentBrandIds.Contains(x.ProductType.IdBrand)
                                                && !x.IsDeleted && !x.ProductType.IsDeleted)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.Name,
                                            x.Price,
                                            x.Pic1,
                                            BrandId = x.ProductType.IdBrand
                                        })
                                        .OrderByDescending(x => x.Id)
                                        .Take(amountTake ?? 0)
                                        .ToListAsync();
            var productsInforDetail = new List<Core.Models.Client.ProductInforModel>();

            foreach (var product in productsDetail)
            {
                var priceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(product.Price);
                productsInforDetail.Add(new Core.Models.Client.ProductInforModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    PriceVND = product.Price,
                    PriceUSD = priceUSD,
                    BrandId = product.BrandId,
                    Pic = product.Pic1
                });
            }

            foreach (var brandId in currentBrandIds)
            {
                productsInfor.Add(new ProductInforViewModel
                {
                    BrandInfor = BrandSingleton.Instance.BrandInfors.Where(x => x.Id == brandId).FirstOrDefault(),
                    ProductsInforDetail = productsInforDetail.Where(x => x.BrandId == brandId).ToList(),
                });
            }

            return productsInfor;
        }

        public async Task<ProductDetailViewModel> GetDetailProductAsync(int id)
        {
            var productDetail = await _context.ProductDetails
                                        .Where(x => x.Id == id && x.Status == ProductStatus.Available
                                                && !x.IsDeleted && !x.ProductType.IsDeleted)
                                        .Select(x => new ProductDetailViewModel
                                        {
                                            Id = x.Id,
                                            Name = x.Name,
                                            Pic1 = x.Pic1,
                                            Pic2 = x.Pic2,
                                            Pic3 = x.Pic3,
                                            Description = x.Description,
                                            PriceVND = x.Price,
                                            BrandInfor = new BrandInforModel
                                            {
                                                Id = x.ProductType.IdBrand,
                                                Name = x.ProductType.Brand.Name
                                            },
                                            BaseProductInfors = x.Products
                                                            .Where(y => !y.IsDeleted && y.Quantity > 0)
                                                            .Select(y => new BaseProductInfor
                                                            {
                                                                Id = y.Id,
                                                                Quantity = y.Quantity,
                                                                ProductSize = y.Size,
                                                                IsAvailable = true
                                                            })
                                                            .ToList(),
                                            ReviewsDetail = x.ReviewDetails
                                                            .Where(y => !y.IsDeleted && y.ReviewStatus == ReviewStatus.Approved)
                                                            .Select(y => new ReviewDetailViewModel
                                                            {
                                                                Id = y.Id,
                                                                IdProductDetail = x.Id,
                                                                Content = y.Content,
                                                                ReviewTime = y.ReviewTime,
                                                                IdCustomer = y.IdCustomer,
                                                                CustomerFullName = y.Customer.FullName
                                                            })
                                                            .OrderByDescending(y => y.ReviewTime)
                                                            .ToList()
                                        })
                                        .SingleOrDefaultAsync();
            productDetail.PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(productDetail.PriceVND);

            var availableSize = productDetail.BaseProductInfors
                                        .Select(x => x.ProductSize)
                                        .ToArray();
            var productSizes = Enum.GetValues(typeof(ProductSize))
                           .Cast<ProductSize>()
                           .ToList();

            foreach (var productSize in productSizes)
            {
                if (!availableSize.Contains(productSize))
                {
                    productDetail.BaseProductInfors.Add(new BaseProductInfor
                    {
                        ProductSize = productSize
                    });
                }
            }
            productDetail.BaseProductInfors = productDetail.BaseProductInfors.OrderBy(x => (int)x.ProductSize).ToList();

            return productDetail;
        }

        public async Task<List<Core.Models.Client.ProductInforModel>> GetCurrentProductsInforAsync(int amountTake)
        {
            var products = await _context.ProductDetails
                                         .Where(x => !x.IsDeleted)
                                         .Select(x => new ProductInforModel
                                         {
                                             Id = x.Id,
                                             Name = x.Name,
                                             PriceVND = x.Price,
                                             Pic = x.Pic1
                                         })
                                        .Take(amountTake)
                                        .ToListAsync();

            foreach (var product in products)
            {
                product.PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(product.PriceVND);
            }

            return products;
        }

        public async Task CreateReviewDetailAsync(ReviewDetailModel reviewDetail, ClaimsPrincipal user)
        {
            string email = user.FindFirst(ClaimTypes.Email).Value;
            string phone = user.FindFirst(ClaimTypes.MobilePhone).Value;
            var idCustomer = await _context.Customers
                                    .Where(x => !x.IsDeleted && x.Email == email && x.PhoneNumber == phone)
                                    .Select(x => x.Id)
                                    .FirstOrDefaultAsync();

            var reviewDetailEntity = new ReviewDetailEntity
            {
                Content = reviewDetail.Content,
                ReviewTime = DateTime.Now,
                IdCustomer = idCustomer,
                ReviewStatus = ReviewStatus.Waiting,
                IdProductDetail = reviewDetail.IdProductDetail
            };

            _context.ReviewDetails.Add(reviewDetailEntity);
            await _context.SaveChangesAsync();
        }

        public async Task InitBrands()
        {
            var brandInfors = await _context.Brands
                                .Where(x => !x.IsDeleted)
                                .Select(x => new BrandInforModel
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Pic = x.Pic
                                })
                                .ToListAsync();
            BrandSingleton.Instance.Init(brandInfors);
        }

        public async Task<TypeOfBrandInforModel> GetTypesOfBrandAsync(int brandId)
        {
            var brandInfor = await _context.Brands
                                    .Where(x => x.Id == brandId && !x.IsDeleted)
                                    .Select(x => new
                                    {
                                        BrandId = x.Id,
                                        Type = x.ProductTypes,
                                        BrandName = x.Name,
                                        x.Pic
                                    }).FirstOrDefaultAsync();

            var types = new TypeOfBrandInforModel
            {
                BrandInfor = new BrandInforModel
                {
                    Id = brandInfor.BrandId,
                    Name = brandInfor.BrandName,
                    Pic = brandInfor.Pic
                },
                TypeInfors = brandInfor.Type.Select(x => new TypeInforModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };

            return types;
        }

        public async Task<ProductsViewModel> GetProductsByBrandAsync(int brandId, int? typeId)
        {
            var productsQuery = _context.ProductDetails.Where(x => x.ProductType.IdBrand == brandId
                                                                && !x.IsDeleted)
                                        .AsQueryable();
            var amountProduct = await productsQuery.CountAsync();

            if (typeId != null)
            {
                productsQuery = productsQuery.Where(x => x.IdProductType == typeId);
            }

            var productsInfor = await productsQuery.Select(x => new ProductInforModel
            {
                Id = x.Id,
                Name = x.Name,
                PriceVND = x.Price,
                Pic = x.Pic1
            })
            .ToListAsync();

            foreach (var product in productsInfor)
            {
                product.PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(product.PriceVND);
            }

            return new ProductsViewModel
            {
                AmountProduct = amountProduct,
                ProductsInfor = productsInfor
            };
        }

        public async Task<IEnumerable<BrandInforModel>> GetBrandAsync()
            => await _context.Brands.Where(x => !x.IsDeleted).Select(x => new BrandInforModel { Id = x.Id, Name = x.Name, Pic = x.Pic }).ToArrayAsync();

        public async Task<IEnumerable<ProductDetailModel>> GetPopularProductsAsync()
        {
            var userId = _currentUserService.Current?.UserId;
            var popularProducts = await _context.Products
                .Where(x => !x.IsDeleted && x.OrderDetails.Any() && !x.ProductDetail.IsDeleted && x.ProductDetail.Status == ProductStatus.Available)
                .Select(x => new ProductDetailModel
                {
                    Id = x.IdProductDetail,
                    Name = x.Name,
                    PriceVND = x.ProductDetail.Price,
                    Pic = x.ProductDetail.Pic1.Replace("Product", "Mobile")
                })
                .Distinct()
                .ToListAsync();

            var favoriteProductDetailIds = new List<int>();

            if (userId != null)
            {
                favoriteProductDetailIds = await _context.FavoriteProducts
                    .Where(x => !x.IsDeleted && x.IdCustomer == userId)
                    .Select(x => x.IdProductDetail)
                    .ToListAsync();
            }

            foreach (var product in popularProducts)
            {
                product.IsFavorite = favoriteProductDetailIds.Contains(product.Id);
                product.PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(product.PriceVND);
            }

            return popularProducts;
        }

        public async Task<ProductModel> GetProductByIdDetailAsync(int idProductDetail)
        {
            var userId = _currentUserService.Current?.UserId;
            var product = await _context.ProductDetails
                .Where(x => !x.IsDeleted && x.Status == ProductStatus.Available && x.Id == idProductDetail)
                .Select(x => new ProductModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PriceVND = x.Price,
                    Pic = x.Pic1.Replace("Product", "Mobile"),
                    Pic2 = x.Pic2.Replace("Product", "Mobile"),
                    Pic3 = x.Pic3.Replace("Product", "Mobile"),
                    Description = x.Description,
                    ProductSizes = x.Products
                        .Where(y => !y.IsDeleted && y.Quantity > 0)
                        .Select(y => new BaseProductInfor
                        {
                            Id = y.Id,
                            IsAvailable = true,
                            ProductSize = y.Size,
                            Size = (int)y.Size,
                            Quantity = y.Quantity,
                        }),
                })
                .FirstOrDefaultAsync();

            if (userId != null)
            {
                product.IsFavorite = await _context.FavoriteProducts
                    .AnyAsync(x => !x.IsDeleted && x.IdCustomer == userId && x.IdProductDetail == product.Id);
            }

            product.PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(product.PriceVND);

            return product;
        }

        public async Task<IEnumerable<ProductDetailModel>> GetFavoriteProductsAsync()
        {
            var userId = _currentUserService.Current?.UserId;

            if (userId == null)
                throw new UserFriendlyException(ErrorCode.Unauthorized);

            var favoriteProductDetailIds = await _context.FavoriteProducts
                    .Where(x => !x.IsDeleted && x.IdCustomer == userId)
                    .Select(x => x.IdProductDetail)
                    .ToListAsync();

            var favoriteProducts = await _context.ProductDetails
                .Where(x => !x.IsDeleted
                    && favoriteProductDetailIds.Contains(x.Id)
                    && x.Status == ProductStatus.Available)
                .Select(x => new ProductDetailModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PriceVND = x.Price,
                    Pic = x.Pic1.Replace("Product", "Mobile"),
                    IsFavorite = true
                })
                .Distinct()
                .ToListAsync();

            foreach (var product in favoriteProducts)
            {
                product.PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(product.PriceVND);
            }

            return favoriteProducts;
        }

        public async Task<IEnumerable<ProductDetailModel>> GetProductsByIdBrandAsync(int idBrand)
        {
            var userId = _currentUserService.Current?.UserId;
            var productOfBrand = await _context.ProductDetails
                .Where(x => !x.IsDeleted && x.ProductType.IdBrand == idBrand && x.Status == ProductStatus.Available)
                .Select(x => new ProductDetailModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PriceVND = x.Price,
                    Pic = x.Pic1.Replace("Product", "Mobile"),
                })
                .Distinct()
                .ToListAsync();

            var favoriteProductDetailIds = new List<int>();

            if (userId != null)
            {
                favoriteProductDetailIds = await _context.FavoriteProducts
                    .Where(x => !x.IsDeleted && x.IdCustomer == userId)
                    .Select(x => x.IdProductDetail)
                    .ToListAsync();
            }

            foreach (var product in productOfBrand)
            {
                product.IsFavorite = favoriteProductDetailIds.Contains(product.Id);
                product.PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(product.PriceVND);
            }

            return productOfBrand;
        }

        public async Task<IEnumerable<ProductDetailModel>> GetProductsByFilterAsync(string searchTerm)
        {
            var userId = _currentUserService.Current?.UserId;
            var productOfBrand = await _context.ProductDetails
                .Where(x => !x.IsDeleted && x.Status == ProductStatus.Available
                        && (
                            x.Name.ToLower().Contains(searchTerm.ToLower())
                        || x.ProductType.Name.ToLower().Contains(searchTerm.ToLower())
                        || x.ProductType.Brand.Name.ToLower().Contains(searchTerm.ToLower())
                            )
                        )
                .Select(x => new ProductDetailModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PriceVND = x.Price,
                    Pic = x.Pic1.Replace("Product", "Mobile"),
                })
                .Distinct()
                .ToListAsync();

            var favoriteProductDetailIds = new List<int>();

            if (userId != null)
            {
                favoriteProductDetailIds = await _context.FavoriteProducts
                    .Where(x => !x.IsDeleted && x.IdCustomer == userId)
                    .Select(x => x.IdProductDetail)
                    .ToListAsync();
            }

            foreach (var product in productOfBrand)
            {
                product.IsFavorite = favoriteProductDetailIds.Contains(product.Id);
                product.PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(product.PriceVND);
            }

            return productOfBrand;
        }
    }
}
