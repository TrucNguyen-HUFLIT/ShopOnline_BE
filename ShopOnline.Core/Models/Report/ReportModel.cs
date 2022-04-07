using System.Collections.Generic;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.Report
{
    public class ReportViewModel
    {
        public IPagedList<ReportModel> Reports { get; set; }
    }

    public class ReportModel
    {
        public string ProductName { get; set; }
        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<QuantityPurchasedSizeModel> QuantityPurchasedSizes { get; set; }
        public int TotalExtraFree { get; set; }
        public int TotalBasePrice { get; set; }
        public int TotalPrice { get; set; }
        public int TotalProfit { get; set; }
    }

    public class QuantityPurchasedSizeModel
    {
        public ProductSize Size { get; set; }
        public int QuantityPurchased { get; set; }
    }
}
