using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business;
using ShopOnline.Business.Customer;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Models;
using ShopOnline.Core.Models.Client;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Controllers.Customer
{
    public class CartController : ApiController
    {
        private readonly ICartBusiness _cartBusiness;
        private readonly IUserBusiness _userBusiness;

        public CartController(ICartBusiness cartBusiness,
                IUserBusiness userBusiness)
        {
            _cartBusiness = cartBusiness;
            _userBusiness = userBusiness;
        }

        [HttpGet("ProductCart")]
        public async Task<IActionResult> ProductCartAsync()
        {
            var productCart = _cartBusiness.GetProductsCart();

            return Ok(productCart);
        }

        [Authorize(Roles = ROLE.CUSTOMER)]
        [HttpGet("CheckOut")]
        public async Task<IActionResult> CheckOutAsync()
        {
            var productCart = _cartBusiness.GetProductsCart();
            var userInfor = _userBusiness.LoadInforUser(User);
            var response = new ProductCartViewModel
            {
                ProductCarts = productCart,
                UserInfor = userInfor
            };

            return Ok(response);
        }

        [Authorize(Roles = ROLE.CUSTOMER)]
        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOutAsync(PaymentMethod paymentMethod, string address)
        {
            int newOrderId = await _cartBusiness.CheckOutAsync(User, paymentMethod, address);
            return Ok(newOrderId);
        }

        [Authorize(Roles = ROLE.CUSTOMER)]
        [HttpGet("DigitalPayment")]
        public async Task<IActionResult> DigitalPayment(int id)
        {
            var orderInfor = await _cartBusiness.GetOrderById(id);
            return Ok(orderInfor);
        }

        [Authorize(Roles = ROLE.CUSTOMER)]
        [HttpGet("ShipCODPayment")]
        public async Task<IActionResult> ShipCODPayment(int id)
        {
            var orderInfor = await _cartBusiness.GetOrderById(id);
            return Ok(orderInfor);
        }

        [HttpPost("AddProductToCart")]
        public async Task<IActionResult> AddProductToCartAsync(int idProduct, int quantity)
        {
            await _cartBusiness.AddProductToCartAsync(idProduct, quantity);
            return Ok();
        }

        [HttpPost("ReduceProductFromCart")]
        public async Task<IActionResult> ReduceProductFromCartAsync(int idProduct, int? quantity)
        {
            await _cartBusiness.ReduceProductFromCartAsync(idProduct, quantity);
            return Ok();
        }

        [HttpPost("RemoveProductFromCart")]
        public async Task<IActionResult> RemoveProductFromCartAsync(int idProduct)
        {
            await _cartBusiness.RemoveProductFromCartAsync(idProduct);
            return Ok();
        }

        [HttpPost("RemoveAllProductFromCart")]
        public async Task<IActionResult> RemoveAllProductFromCartAsync()
        {
            await _cartBusiness.RemoveAllProductFromCartAsync();
            return Ok();
        }
    }
}
