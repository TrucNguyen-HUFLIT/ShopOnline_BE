using System.Collections.Generic;
using X.PagedList;

namespace ShopOnline.Core.Models.Client
{
    public class ProductHomePageViewModel
    {
        public List<ProductInforViewModel> ProductInfors { get; set; }
    }

    public class ProductInforViewModel
    {
        public BrandInforModel BrandInfor { get; set; }
        public List<ProductInforModel> ProductsInforDetail { get; set; }

    }

    public class ProductsBrandPageViewModel
    {
        public int AmountProduct { get; set; }
        public TypeOfBrandInforModel TypeOfBrand { get; set; }
        public IPagedList<ProductInforModel> ProductsInfor { get; set; }
    }

    public class ProductsViewModel
    {
        public List<ProductInforModel> ProductsInfor { get; set; }
        public int AmountProduct { get; set; }
    }

    public class ProductInforModel
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int PriceVND { get; set; }
        public int PriceUSD { get; set; }
        public string Pic { get; set; }
    }

    public class InforModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BrandInforModel : InforModel
    {

    }

    public class TypeInforModel : InforModel
    {

    }

    public class TypeOfBrandInforModel
    {
        public BrandInforModel BrandInfor { get; set; }
        public List<TypeInforModel> TypeInfors { get; set; }
    }
}
