using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.Product
{
    public class ProductDetailModel
    {
        public ProductDetailInfor productDetail { get; set; }
        public List<ProductTypeInfor> ListProductType { get; set; }
        public IPagedList<ProductDetailInfor> ListProductDetail { get; set; }
    }

    public class ProductDetailInfor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pic1 { get; set; }
        public string Pic2 { get; set; }
        public string Pic3 { get; set; }
        public int Price { get; set; }
        public int BasePrice { get; set; }
        public ProductStatus Status { get; set; }
        [Display(Name = "Type")]
        public int IdProductType { get; set; }
        public string Description { get; set; }
    }

    public class ProductDetailCreate : ProductDetailInfor
    {

        [NotMapped]
        public IFormFile UploadPic1 { get; set; }
        [NotMapped]
        public IFormFile UploadPic2 { get; set; }
        [NotMapped]
        public IFormFile UploadPic3 { get; set; }

    }
    public class ProductDetailUpdate : ProductDetailCreate
    {

    }

    public class ProductDetailCreateViewModel
    {
        public ProductDetailCreate ProductDetailCreate { get; set; }
        public List<ProductTypeInfor> ListProductType { get; set; }
    }

    public class ProductDetailUpdateViewModel
    {
        public ProductDetailUpdate ProductDetailUpdate { get; set; }
        public List<ProductTypeInfor> ListProductType { get; set; }
    }
}
