using ShopOnline.Core.Models.Customer;
using System;
using System.Collections.Generic;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.Order
{
    public class OrderModel
    {
        public OrderInfor orderInfor { get; set; }
        public List<CustomerInfor> ListCustomer { get; set; }
        public IPagedList<OrderInfor> ListOrder { get; set; }
    }
    public class OrderInfor
    {
        public int Id { get; set; }
        public DateTime OrderDay { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public int IdCustomer { get; set; }
        public int ExtraFee { get; set; }
        public int TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public PaymentMethod Payment { get; set; }
        public string Address { get; set; }
    }

    public class OrderInforShipperModel
    {
        public OrderInforShipper OrderInforShipper { get; set; }
        public List<CustomerInfor> ListCustomer { get; set; }
        public IPagedList<OrderInforShipper> ListOrderInforShipper { get; set; }
    }

    public class OrderInforShipper : OrderInfor
    {

    }
}