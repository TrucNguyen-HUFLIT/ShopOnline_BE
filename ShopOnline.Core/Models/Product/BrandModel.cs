using X.PagedList;

namespace ShopOnline.Core.Models.Product
{
    public class BrandModel
    {
        public IPagedList<BrandInfor> ListBrand { get; set; }
    }
    public class BrandInfor
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
    }
    public class BrandCreate
    {
        public string BrandName { get; set; }
    }

    public class BrandInforViewModel
    {
        public BrandInfor BrandInfor;
    }
    public class BrandCreateViewModel
    {

        public BrandCreate BrandCreate;
    }
}
