using ShopOnline.Core.Models.Customer;
using System;
using System.Collections.Generic;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.HistoryOrder
{
    public class HistoryOrderModel
    {
        public HistoryOrderInfor HistoryOrderInfor { get; set; }
        public IPagedList<HistoryOrderInfor> ListHistoryOrder { get; set; }
    }

    public class HistoryOrderInfor
    {
        public int Id { get; set; }
        public DateTime OrderDay { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public PaymentMethod Payment { get; set; }
        public int ExtraFee { get; set; }
        public bool IsPaid { get; set; }
        public string Address { get; set; }
        public int TotalPrice { get; set; }
    }

    public class HistoryOrderShipperInfor : HistoryOrderInfor
    {
        public int IdCustomer { get; set; }
    }

    public class HistoryOrderShipperModel
    {
        public HistoryOrderShipperInfor HistoryOrderShipperInfor { get; set; }
        public List<CustomerInfor> ListCustomer { get; set; }
        public IPagedList<HistoryOrderShipperInfor> ListHistoryOrderShipper { get; set; }
    }
}
