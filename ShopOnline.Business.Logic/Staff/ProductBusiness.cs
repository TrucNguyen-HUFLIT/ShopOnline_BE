using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Business.Staff;
using ShopOnline.Core;
using ShopOnline.Core.Entities;
using ShopOnline.Core.Exceptions;
using ShopOnline.Core.Models.Enum;
using ShopOnline.Core.Models.Product;
using ShopOnline.Data.Repositories.Product;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ShopOnline.Business.Logic.Staff
{
    public class ProductBusiness : IProductBusiness
    {
        private readonly MyDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment hostEnvironment;
        public ProductBusiness(MyDbContext context, IWebHostEnvironment hostEnvironment, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
            this.hostEnvironment = hostEnvironment;
        }

        public async Task CreateBrandAsync(BrandCreate brandCreate)
        {
            var brand = await _context.Brands.Where(x => x.Name == brandCreate.BrandName && !x.IsDeleted).Select(x => x.Name).FirstOrDefaultAsync();
            if (brand == null)
            {
                var brandEntity = new BrandEntity
                {
                    Name = brandCreate.BrandName,
                };
                _context.Brands.Add(brandEntity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException(ErrorCode.BrandExisted);
            }
        }

        public async Task<bool> EditBrandAsync(BrandInfor brandInfor)
        {
            var brandEntity = await _context.Brands.Where(x => x.Id == brandInfor.Id && !x.IsDeleted).FirstOrDefaultAsync();
            if (brandEntity == null)
            {
                throw new UserFriendlyException(ErrorCode.BrandNotExisted);
            }

            var brandName = brandInfor.BrandName.ToLower().Trim();
            if (brandEntity.Name.ToLower().Trim() != brandName)
            {
                bool isExistedBrandName = await _context.Brands.AnyAsync(x => x.Name.ToLower().Trim() == brandName && !x.IsDeleted);
                if (isExistedBrandName) throw new UserFriendlyException(ErrorCode.BrandExisted);
                brandEntity.Name = brandInfor.BrandName;
            }
            _context.Brands.Update(brandEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public BrandInfor GetBrandByIdAsync(int id)
        {
            var brand = _context.Brands.Where(x => x.Id == id && !x.IsDeleted).Select(x => new BrandInfor
            {
                Id = x.Id,
                BrandName = x.Name,
            }).FirstOrDefault();

            return brand;
        }

        public async Task<IPagedList<BrandInfor>> GetListBrandAsync(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var listBrand = new List<BrandInfor>();
            var brands = await _context.Brands.Where(x => !x.IsDeleted).ToListAsync();
            if (brands != null)
            {
                foreach (var brand in brands)
                {
                    var brandInfor = new BrandInfor
                    {
                        Id = brand.Id,
                        BrandName = brand.Name
                    };
                    listBrand.Add(brandInfor);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    listBrand = listBrand.Where(s => s.BrandName.ToLower().Contains(searchString.ToLower())).ToList();
                }
                listBrand = sortOrder switch
                {
                    "name_desc" => listBrand.OrderByDescending(x => x.BrandName).ToList(),
                    "name" => listBrand.OrderBy(x => x.BrandName).ToList(),
                    "id_desc" => listBrand.OrderByDescending(x => x.Id).ToList(),
                    _ => listBrand.OrderBy(x => x.Id).ToList(),
                };
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return listBrand.ToPagedList(pageNumber, pageSize);
            }

            return null;
        }

        public async Task<IPagedList<ProductTypeInfor>> GetListProductTypeAsync(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var listProductType = new List<ProductTypeInfor>();
            var productTypes = await _context.ProductTypes.Where(x => !x.IsDeleted).ToListAsync();
            if (productTypes != null)
            {
                foreach (var product in productTypes)
                {
                    var productType = new ProductTypeInfor
                    {
                        Id = product.Id,
                        Name = product.Name,
                        IdBrand = product.IdBrand
                    };
                    listProductType.Add(productType);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    listProductType = listProductType.Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
                }
                listProductType = sortOrder switch
                {
                    "name_desc" => listProductType.OrderByDescending(x => x.Name).ToList(),
                    "name" => listProductType.OrderBy(x => x.Name).ToList(),
                    "id_desc" => listProductType.OrderByDescending(x => x.Id).ToList(),
                    _ => listProductType.OrderBy(x => x.Id).ToList(),
                };
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return listProductType.ToPagedList(pageNumber, pageSize);
            }

            return null;
        }

        public ProductTypeInfor GetProductTypeByIdAsync(int id)
        {
            var productType = _context.ProductTypes.Where(x => x.Id == id && !x.IsDeleted).Select(x => new ProductTypeInfor
            {
                Id = x.Id,
                Name = x.Name,
                IdBrand = x.IdBrand,
            }).FirstOrDefault();

            return productType;
        }

        public async Task<bool> EditProductTypeAsync(ProductTypeInfor productType)
        {
            var productTypeEntity = await _context.ProductTypes.Where(x => x.Id == productType.Id && !x.IsDeleted).FirstOrDefaultAsync();
            var productTypeName = productType.Name.ToLower().Trim();

            if (productTypeEntity.Name.ToLower().Trim() != productTypeName)
            {
                bool isExistedProductTypeName = await _context.ProductTypes.AnyAsync(x => x.Name.ToLower().Trim() == productTypeName && !x.IsDeleted);
                if (isExistedProductTypeName) throw new UserFriendlyException(ErrorCode.ProductTypeExisted);
                productTypeEntity.Name = productType.Name;

            }
            productTypeEntity.IdBrand = productType.IdBrand;
            _context.ProductTypes.Update(productTypeEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BrandInfor>> GetListBrand()
        {
            var brands = await _context.Brands.Where(x => !x.IsDeleted).Select(x => new BrandInfor
            {
                BrandName = x.Name,
                Id = x.Id,
            }).ToListAsync();
            return brands;
        }

        public async Task CreateProductTypeAsync(ProductTypeInfor productTypeInfor)
        {
            var productType = await _context.ProductTypes.Where(x => x.Name == productTypeInfor.Name && !x.IsDeleted).Select(x => x.Name).FirstOrDefaultAsync();
            if (productType == null)
            {
                var productTypeEntity = new ProductTypeEntity
                {
                    Name = productTypeInfor.Name,
                    IdBrand = productTypeInfor.IdBrand,
                };
                _context.ProductTypes.Add(productTypeEntity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException(ErrorCode.ProductTypeExisted);
            }
        }

        public async Task<IPagedList<ProductDetailInfor>> GetListProductDetailAsync(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var listProductDetail = new List<ProductDetailInfor>();
            var productDetails = await _context.ProductDetails.Where(x => !x.IsDeleted).ToListAsync();
            if (productDetails != null && productDetails.Any())
            {
                foreach (var productDetail in productDetails)
                {
                    var productDetailInfor = new ProductDetailInfor
                    {
                        Id = productDetail.Id,
                        Name = productDetail.Name,
                        Pic1 = productDetail.Pic1,
                        Price = productDetail.Price,
                        Status = productDetail.Status,
                        IdProductType = productDetail.IdProductType,
                    };
                    listProductDetail.Add(productDetailInfor);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    listProductDetail = listProductDetail.Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
                }
                listProductDetail = sortOrder switch
                {
                    "name_desc" => listProductDetail.OrderByDescending(x => x.Name).ToList(),
                    "name" => listProductDetail.OrderBy(x => x.Name).ToList(),
                    "id_desc" => listProductDetail.OrderByDescending(x => x.Id).ToList(),
                    _ => listProductDetail.OrderBy(x => x.Id).ToList(),
                };
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return listProductDetail.ToPagedList(pageNumber, pageSize);
            }

            return null;
        }

        public async Task<List<ProductTypeInfor>> GetListProductType()
        {
            var productTypes = await _context.ProductTypes.Where(x => !x.IsDeleted).Select(x => new ProductTypeInfor
            {
                Name = x.Name,
                Id = x.Id,
                IdBrand = x.IdBrand
            }).ToListAsync();

            return productTypes;
        }

        public async Task<List<ProductDetailInfor>> GetListProductDetail()
        {
            var productDetails = await _context.ProductDetails.Where(x => !x.IsDeleted && x.Status == AppEnum.ProductStatus.Available).Select(x => new ProductDetailInfor
            {
                Name = x.Name,
                Id = x.Id,
                Pic1 = x.Pic1,
                Price = x.Price,
                BasePrice = x.BasePrice,
                Status = x.Status,
                IdProductType = x.IdProductType,
            }).ToListAsync();

            return productDetails;
        }

        public async Task CreateProductDetailAsync(ProductDetailCreate productDetailCreate)
        {
            var productDetail = await _context.ProductDetails.Where(x => x.Name == productDetailCreate.Name && !x.IsDeleted).Select(x => x.Name).FirstOrDefaultAsync();
            if (productDetail == null)
            {
                var productDetailEntity = new ProductDetailEntity
                {
                    Name = productDetailCreate.Name,
                    Description = productDetailCreate.Description,
                    Status = productDetailCreate.Status,
                    Price = productDetailCreate.Price,
                    BasePrice = productDetailCreate.BasePrice,
                    IdProductType = productDetailCreate.IdProductType,
                };

                #region Save Image from wwwroot/img
                string wwwRootPath = hostEnvironment.WebRootPath;

                string fileName1 = Path.GetFileNameWithoutExtension(productDetailCreate.UploadPic1.FileName);
                string extension1 = Path.GetExtension(productDetailCreate.UploadPic1.FileName);
                productDetailEntity.Pic1 = "/img/Product/" + fileName1 + extension1;
                string path1 = Path.Combine(wwwRootPath + "/img/Product/", fileName1 + extension1);
                using (var fileStream = new FileStream(path1, FileMode.Create))
                {
                    await productDetailCreate.UploadPic1.CopyToAsync(fileStream);
                }

                if (productDetailCreate.UploadPic2 != null)
                {
                    string fileName2 = Path.GetFileNameWithoutExtension(productDetailCreate.UploadPic2.FileName);
                    string extension2 = Path.GetExtension(productDetailCreate.UploadPic2.FileName);
                    productDetailEntity.Pic2 = "/img/Product/" + fileName2 + extension2;
                    string path2 = Path.Combine(wwwRootPath + "/img/Product/", fileName2 + extension2);
                    using (var fileStream = new FileStream(path2, FileMode.Create))
                    {
                        await productDetailCreate.UploadPic2.CopyToAsync(fileStream);
                    }
                }

                if (productDetailCreate.UploadPic3 != null)
                {
                    string fileName3 = Path.GetFileNameWithoutExtension(productDetailCreate.UploadPic3.FileName);
                    string extension3 = Path.GetExtension(productDetailCreate.UploadPic3.FileName);
                    productDetailEntity.Pic3 = "/img/Product/" + fileName3 + extension3;
                    string path3 = Path.Combine(wwwRootPath + "/img/Product/", fileName3 + extension3);
                    using (var fileStream = new FileStream(path3, FileMode.Create))
                    {
                        await productDetailCreate.UploadPic3.CopyToAsync(fileStream);
                    }
                }
                #endregion

                _context.ProductDetails.Add(productDetailEntity);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UserFriendlyException(ErrorCode.ProductDetailExisted);
            }

        }

        public async Task<bool> UpdateProductDetailAsync(ProductDetailUpdate productDetailUpdate)
        {
            var productDetailEntity = await _context.ProductDetails.Where(x => x.Id == productDetailUpdate.Id && !x.IsDeleted).FirstOrDefaultAsync();
            var productDetailName = productDetailUpdate.Name.ToLower().Trim();

            if (productDetailEntity.Name.ToLower().Trim() != productDetailName)
            {
                bool isExistedProductDetailName = await _context.ProductDetails.AnyAsync(x => x.Name.ToLower().Trim() == productDetailName && !x.IsDeleted);
                if (isExistedProductDetailName) throw new UserFriendlyException(ErrorCode.ProductDetailExisted);
                productDetailEntity.Name = productDetailUpdate.Name;

            }
            productDetailEntity.Description = productDetailUpdate.Description;
            productDetailEntity.Price = productDetailUpdate.Price;
            productDetailEntity.BasePrice = productDetailUpdate.BasePrice;
            productDetailEntity.Status = productDetailUpdate.Status;
            productDetailEntity.IdProductType = productDetailUpdate.IdProductType;

            #region Save Image from wwwroot/img
            string wwwRootPath = hostEnvironment.WebRootPath;

            string fileName1, fileName2, fileName3;
            string extension1, extension2, extension3;
            if (productDetailUpdate.UploadPic1 != null)
            {
                fileName1 = Path.GetFileNameWithoutExtension(productDetailUpdate.UploadPic1.FileName);
                extension1 = Path.GetExtension(productDetailUpdate.UploadPic1.FileName);
                productDetailEntity.Pic1 = "/img/Product/" + fileName1 + extension1;
                string path1 = Path.Combine(wwwRootPath + "/img/Product/", fileName1 + extension1);
                using (var fileStream = new FileStream(path1, FileMode.Create))
                {
                    await productDetailUpdate.UploadPic1.CopyToAsync(fileStream);
                }
            }
            if (productDetailUpdate.UploadPic2 != null)
            {
                fileName2 = Path.GetFileNameWithoutExtension(productDetailUpdate.UploadPic2.FileName);
                extension2 = Path.GetExtension(productDetailUpdate.UploadPic2.FileName);
                productDetailEntity.Pic2 = "/img/Product/" + fileName2 + extension2;
                string path2 = Path.Combine(wwwRootPath + "/img/Product/", fileName2 + extension2);
                using (var fileStream = new FileStream(path2, FileMode.Create))
                {
                    await productDetailUpdate.UploadPic2.CopyToAsync(fileStream);
                }
            }

            if (productDetailUpdate.UploadPic3 != null)
            {
                fileName3 = Path.GetFileNameWithoutExtension(productDetailUpdate.UploadPic3.FileName);
                extension3 = Path.GetExtension(productDetailUpdate.UploadPic3.FileName);
                productDetailEntity.Pic1 = "/img/Product/" + fileName3 + extension3;
                string path3 = Path.Combine(wwwRootPath + "/img/Product/", fileName3 + extension3);
                using (var fileStream = new FileStream(path3, FileMode.Create))
                {
                    await productDetailUpdate.UploadPic3.CopyToAsync(fileStream);
                }
            }

            #endregion

            _context.ProductDetails.Update(productDetailEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public ProductDetailUpdate GetProductDetailByIdAsync(int id)
        {
            var productDetail = _context.ProductDetails.Where(x => x.Id == id && !x.IsDeleted).Select(x => new ProductDetailUpdate
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                IdProductType = x.IdProductType,
                Pic1 = x.Pic1,
                Pic2 = x.Pic2,
                Pic3 = x.Pic3,
                Price = x.Price,
                BasePrice = x.BasePrice,
                Status = x.Status,
            }).FirstOrDefault();

            return productDetail;
        }

        public async Task<ProductUpdate> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.Where(x => x.Id == id && !x.IsDeleted).Select(x => new ProductUpdate
            {
                Id = x.Id,
                Name = x.Name,
                Quantity = x.Quantity,
                IdProductDetail = x.IdProductDetail,
                Size = x.Size,
            }).FirstOrDefaultAsync();

            return product;
        }

        public async Task<IPagedList<ProductInfor>> GetListProductAsync(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var listProduct = new List<ProductInfor>();
            var products = await _productRepository.Get().ToListAsync();

            if (products != null && products.Any())
            {
                foreach (var product in products)
                {
                    var productInfor = new ProductInfor
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Quantity = product.Quantity,
                        Size = product.Size,
                        IdProductDetail = product.IdProductDetail,
                        //Pic1 = product.ProductDetail.Pic1,
                        //Pic2 = product.ProductDetail.Pic2,
                        //Pic3 = product.ProductDetail.Pic3,

                    };
                    listProduct.Add(productInfor);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    listProduct = listProduct.Where(s => s.Name.ToLower().Contains(searchString.ToLower())).ToList();
                }
                listProduct = sortOrder switch
                {
                    "name_desc" => listProduct.OrderByDescending(x => x.Name).ToList(),
                    "name" => listProduct.OrderBy(x => x.Name).ToList(),
                    "id_desc" => listProduct.OrderByDescending(x => x.Id).ToList(),
                    _ => listProduct.OrderBy(x => x.Id).ToList(),
                };
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return listProduct.ToPagedList(pageNumber, pageSize);
            }

            return null;
        }

        public async Task CreateProductAsync(ProductCreate productCreate)
        {
            var product = await _productRepository.Get()
                            .Where(x => x.IdProductDetail == productCreate.IdProductDetail
                            && x.Size == productCreate.Size)
                            .FirstOrDefaultAsync();

            if (product == null)
            {
                var productEntity = new ProductEntity
                {
                    Name = productCreate.Name,
                    Quantity = productCreate.Quantity,
                    Size = productCreate.Size,
                    IdProductDetail = productCreate.IdProductDetail,
                };

                _productRepository.Add(productEntity);
            }
            else
            {
                product.Quantity += productCreate.Quantity;
                _productRepository.Update(product);
            }

            await _productRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateProductAsync(ProductUpdate productUpdate)
        {
            var productEntity = await _productRepository.Get().Where(x => x.Id == productUpdate.Id).FirstOrDefaultAsync();

            if (productEntity != null)
            {
                productEntity.Quantity = productUpdate.Quantity;

                _productRepository.Update(productEntity);
                await _productRepository.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.Get().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (product != null)
            {
                var isCannotDelete = await _productRepository.Get().Where(x => x.Id == id).AnyAsync(x => x.OrderDetails.Any(y => y.Order.StatusOrder == AppEnum.StatusOrder.Accepted
                                                                              || y.Order.StatusOrder == AppEnum.StatusOrder.Processing
                                                                              || y.Order.StatusOrder == AppEnum.StatusOrder.Delivering));

                if (isCannotDelete)
                {
                    throw new UserFriendlyException(ErrorCode.CannotDeleteProduct);
                }

                _productRepository.SoftDelete(product);
                await _productRepository.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await _context.Brands.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();

            if (brand != null)
            {
                var isCannotDelete = await _context.ProductTypes.Where(x => x.IdBrand == id).AnyAsync(x => !x.IsDeleted || x.ProductDetails.Any(y => !y.IsDeleted || y.Products.Any(z => !z.IsDeleted)));

                if (isCannotDelete)
                {
                    throw new UserFriendlyException(ErrorCode.CannotDelete);
                }

                brand.IsDeleted = true;
                _context.Brands.Update(brand);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteProductTypeAsync(int id)
        {
            var productType = await _context.ProductTypes.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();

            if (productType != null)
            {
                var isCannotDelete = await _context.ProductDetails.Where(x => x.IdProductType == id).AnyAsync(x => !x.IsDeleted || x.Products.Any(y => !y.IsDeleted));

                if (isCannotDelete)
                {
                    throw new UserFriendlyException(ErrorCode.CannotDelete);
                }

                productType.IsDeleted = true;
                _context.ProductTypes.Update(productType);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteProductDetailAsync(int id)
        {
            var productDetail = await _context.ProductDetails.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();

            if (productDetail != null)
            {
                productDetail.IsDeleted = true;
                _context.ProductDetails.Update(productDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
