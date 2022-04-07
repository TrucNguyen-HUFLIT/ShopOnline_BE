using System;
using System.Collections.Generic;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime OrderDay { get; set; }
        public StatusOrder StatusOrder { get; set; }
        public int ExtraFee { get; set; }
        public string Address { get; set; }
        public bool IsPaid { get; set; }
        public PaymentMethod Payment { get; set; }

        public int IdCustomer { get; set; }
        public virtual CustomerEntity Customer { get; set; }
        public int? IdShipper { get; set; }
        public virtual ShipperEntity Shipper { get; set; }
        public virtual ICollection<OrderDetailEntity> OrderDetails { get; set; }

    }
}
