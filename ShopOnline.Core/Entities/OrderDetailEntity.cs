namespace ShopOnline.Core.Entities
{
    public class OrderDetailEntity
    {
        public int TotalPrice { get; set; }
        public int TotalBasePrice { get; set; }
        public int QuantityPurchased { get; set; }

        public int IdOrder { get; set; }
        public OrderEntity Order { get; set; }
        public int IdProduct { get; set; }
        public ProductEntity Product { get; set; }
    }
}
