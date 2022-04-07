using ShopOnline.Core.Models.Client;
using ShopOnline.Core.Models.Enum;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopOnline.Business.Customer
{
    public interface ICartBusiness
    {
        Task AddProductToCartAsync(int idProduct, int quantity);
        Task<int> CheckOutAsync(ClaimsPrincipal user, AppEnum.PaymentMethod paymentMethod, string address);
        Task<OrderInfor> GetOrderById(int id);
        List<ProductCartModel> GetProductsCart();
        Task ReduceProductFromCartAsync(int idProduct, int? quantity);
        Task RemoveAllProductFromCartAsync();
        Task RemoveProductFromCartAsync(int idProduct);
    }
}
