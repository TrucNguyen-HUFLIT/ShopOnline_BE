using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using ShopOnline.Business.Customer;
using ShopOnline.Core;
using ShopOnline.Core.Entities;
using ShopOnline.Core.Exceptions;
using ShopOnline.Core.Helpers;
using ShopOnline.Core.Models.Account;
using ShopOnline.Core.Models.Client;
using ShopOnline.Core.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Business.Logic.Customer
{
    public class CartBusiness : ICartBusiness
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MyDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public CartBusiness(IHttpContextAccessor httpContextAccessor,
                            MyDbContext context,
                            ICurrentUserService currentUserService)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _currentUserService = currentUserService;
        }

        public List<ProductCartModel> GetProductsCart()
        {
            string jsonCart = _session.GetString(CART.CART_KEY);
            if (jsonCart != null)
            {
                return JsonConvert.DeserializeObject<List<ProductCartModel>>(jsonCart);
            }
            return new List<ProductCartModel>();
        }

        public async Task AddProductToCartAsync(int idProduct, int quantity)
        {
            var cart = GetProductsCart();
            var productInCart = cart.Where(x => x.Id == idProduct).FirstOrDefault();

            if (productInCart == null)
            {
                productInCart = await _context.Products.Where(x => x.Id == idProduct && !x.IsDeleted)
                                    .Select(x => new ProductCartModel
                                    {
                                        Id = x.Id,
                                        Name = x.Name,
                                        Quantity = x.Quantity,
                                        PriceVND = x.ProductDetail.Price,
                                        BasePrice = x.ProductDetail.BasePrice,
                                        Pic = x.ProductDetail.Pic1,
                                        Size = x.Size,
                                        IdProductDetail = x.IdProductDetail,
                                    })
                                    .FirstOrDefaultAsync();

                if (productInCart == null)
                    throw new UserFriendlyException(ErrorCode.NotFoundInCart);

                productInCart.PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(productInCart.PriceVND);
                cart.Add(productInCart);
            }

            productInCart.SelectedQuantity = quantity == 0
                                    ? productInCart.SelectedQuantity++
                                    : productInCart.SelectedQuantity + quantity;

            if (productInCart.SelectedQuantity > productInCart.Quantity)
                throw new UserFriendlyException(ErrorCode.OutOfStock);

            productInCart.TotalVND = productInCart.PriceVND * productInCart.SelectedQuantity;
            productInCart.TotalUSD = productInCart.PriceUSD * productInCart.SelectedQuantity;
            productInCart.TotalBasePrice = productInCart.BasePrice * productInCart.SelectedQuantity;

            SaveCartSession(cart);
        }

        public Task ReduceProductFromCartAsync(int idProduct, int? quantity)
        {
            var cart = GetProductsCart();
            var productInCart = cart.Where(x => x.Id == idProduct).FirstOrDefault();

            if (productInCart == null)
                throw new UserFriendlyException(ErrorCode.NotFoundInCart);

            productInCart.SelectedQuantity = quantity == null
                                    ? productInCart.SelectedQuantity--
                                    : productInCart.SelectedQuantity - (int)quantity;

            if (productInCart.SelectedQuantity < 0)
                productInCart.SelectedQuantity = 0;

            productInCart.TotalVND = productInCart.PriceVND * productInCart.SelectedQuantity;
            productInCart.TotalUSD = productInCart.PriceUSD * productInCart.SelectedQuantity;
            productInCart.TotalBasePrice = productInCart.BasePrice * productInCart.SelectedQuantity;

            SaveCartSession(cart);

            if (productInCart.SelectedQuantity == 0)
                RemoveProductFromCartAsync(idProduct);

            return Task.CompletedTask;
        }

        public Task RemoveProductFromCartAsync(int idProduct)
        {
            var cart = GetProductsCart();
            var productInCart = cart.Where(x => x.Id == idProduct).FirstOrDefault();

            if (productInCart == null)
                throw new UserFriendlyException(ErrorCode.NotFoundInCart);

            cart.Remove(productInCart);

            SaveCartSession(cart);
            return Task.CompletedTask;
        }

        public Task RemoveAllProductFromCartAsync()
        {
            SaveCartSession(new List<ProductCartModel>());
            return Task.CompletedTask;
        }

        public async Task<int> CheckOutAsync(ClaimsPrincipal user, PaymentMethod paymentMethod, string address)
        {
            string email = user.FindFirst(ClaimTypes.Email).Value;
            string phone = user.FindFirst(ClaimTypes.MobilePhone).Value;
            var orderDetails = new List<OrderDetailEntity>();
            var cart = GetProductsCart();
            if (!cart.Any()) throw new UserFriendlyException(ErrorCode.EmptyCart);

            var productIds = cart.Select(x => x.Id).ToArray();
            var products = await _context.Products.Where(x => !x.IsDeleted && productIds.Contains(x.Id)).ToArrayAsync();
            var customer = await _context.Customers
                                    .Where(x => !x.IsDeleted && x.Email == email && x.PhoneNumber == phone)
                                    .Select(x => new InforAccount
                                    {
                                        Id = x.Id,
                                        Email = x.Email,
                                        FullName = x.FullName,
                                    })
                                    .FirstOrDefaultAsync();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                OrderEntity orderEntity = new()
                {
                    OrderDay = DateTime.Now,
                    StatusOrder = StatusOrder.Processing,
                    IdCustomer = customer.Id,
                    Address = address,
                    IsPaid = false,
                    Payment = paymentMethod,
                    ExtraFee = cart.Sum(x => x.TotalVND) > 5000000 ? 0 : 5000000,
                };
                _context.Orders.Add(orderEntity);
                await _context.SaveChangesAsync();

                foreach (var product in cart)
                {
                    var productEntity = products.Where(x => x.Id == product.Id).FirstOrDefault();
                    int newQuantity = productEntity.Quantity - product.SelectedQuantity;

                    if (newQuantity < 0)
                    {
                        transaction.Rollback();
                        throw new UserFriendlyException(ErrorCode.OutOfStock);
                    }
                    else
                    {
                        productEntity.Quantity = newQuantity;
                    }
                    _context.Products.Update(productEntity);

                    orderDetails.Add(new OrderDetailEntity
                    {
                        IdOrder = orderEntity.Id,
                        IdProduct = product.Id,
                        TotalPrice = product.TotalVND,
                        TotalBasePrice = product.TotalBasePrice,
                        QuantityPurchased = product.SelectedQuantity,
                    });
                }
                _context.OrderDetails.AddRange(orderDetails);
                await SendEmailConfirm(customer, cart, orderEntity.Id, address);
                await _context.SaveChangesAsync();
                transaction.Commit();
                await RemoveAllProductFromCartAsync();
                return orderEntity.Id;
            }
        }

        public async Task CheckOutAsync(CheckOutCartRequestModel model)
        {
            var currentUser = _currentUserService.Current;
            string email = currentUser.Email;
            string phone = currentUser.Phone;
            var orderDetails = new List<OrderDetailEntity>();
            if (!model.ProductCheckOutModels.Any()) throw new UserFriendlyException(ErrorCode.EmptyCart);

            var productIds = model.ProductCheckOutModels.Select(x => x.Id).ToArray();
            var products = await _context.Products.Where(x => !x.IsDeleted && productIds.Contains(x.Id)).Include(x => x.ProductDetail).ToArrayAsync();
            var customer = await _context.Customers
                                    .Where(x => !x.IsDeleted && x.Email == email && x.PhoneNumber == phone)
                                    .Select(x => new InforAccount
                                    {
                                        Id = x.Id,
                                        Email = x.Email,
                                        FullName = x.FullName,
                                    })
                                    .FirstOrDefaultAsync();
            var cart = new List<ProductCartModel>();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                OrderEntity orderEntity = new()
                {
                    OrderDay = DateTime.Now,
                    StatusOrder = StatusOrder.Processing,
                    IdCustomer = customer.Id,
                    Address = model.Address,
                    IsPaid = false,
                    Payment = model.PaymentMethod,
                    ExtraFee = products.Sum(x => x.ProductDetail.Price) > 5000000 ? 0 : 5000000,
                };
                _context.Orders.Add(orderEntity);
                await _context.SaveChangesAsync();

                foreach (var product in products)
                {
                    var quantitySelected = model.ProductCheckOutModels.Where(x => x.Id == product.Id).Select(x=>x.Quantity).FirstOrDefault();
                    int newQuantity = product.Quantity - quantitySelected;

                    if (newQuantity < 0)
                    {
                        transaction.Rollback();
                        throw new UserFriendlyException(ErrorCode.OutOfStock);
                    }
                    else
                    {
                        product.Quantity = newQuantity;
                    }
                    _context.Products.Update(product);

                    var totalPrice = product.ProductDetail.Price * quantitySelected;
                    var totalBasePrice = product.ProductDetail.BasePrice * quantitySelected;

                    cart.Add(new ProductCartModel
                    {
                        Id = product.Id,
                        TotalBasePrice = totalBasePrice,
                        TotalVND = totalPrice,
                        TotalUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(totalPrice),
                        Size = product.Size,
                        Name = product.Name,
                        SelectedQuantity = quantitySelected,
                        PriceVND = product.ProductDetail.Price,
                        BasePrice = product.ProductDetail.BasePrice,
                        PriceUSD = await ConvertCurrencyHelper.ConvertVNDToUSD(product.ProductDetail.Price),
                    });

                    orderDetails.Add(new OrderDetailEntity
                    {
                        IdOrder = orderEntity.Id,
                        IdProduct = product.Id,
                        TotalPrice = totalPrice,
                        TotalBasePrice = totalBasePrice,
                        QuantityPurchased = quantitySelected,
                    });
                }
                _context.OrderDetails.AddRange(orderDetails);
                await SendEmailConfirm(customer, cart, orderEntity.Id, model.Address);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
        }

        public async Task SendEmailConfirm(InforAccount infor, List<ProductCartModel> productCarts, int idOrder, string address)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress from = new("Dreams Store", "dreamsstore.ss@gmail.com");
            message.From.Add(from);

            MailboxAddress to = new(infor.FullName, infor.Email);
            message.To.Add(to);

            message.Subject = "Confirm order from Dreams Store";

            StringBuilder productsInfor = new StringBuilder();

            string inforAccountTemplate = "<div> " +
                "<div style=\"width: 100 %; padding: 24px 0 16px 0; background - color: #f5f5f5; text-align: center;\">" +
                " <div style=\"display: inline-block; width: 90%; max-width: 680px; min-width: 280px; text-align: left; font-family: Roboto, Arial, Helvetica, sans-serif;\">" +
                " <div class=\"adM\" dir=\"ltr\" style=\"height: 0px;\">&nbsp;</div> " +
                "<div class=\"adM\" style=\"display: block; padding: 0 2px;\">&nbsp;</div> " +
                "<div style=\"border-left: 1px solid #F5F5F5; border-right: 1px solid #F5F5F5;\"> " +
                "<div style=\"width: 100%; background-color: #8bc53f; height: 5px;\">&nbsp;</div> " +
                "<div dir=\"ltr\" style=\"padding: 24px 32px 24px 32px; background: #fff; border-right: 1px solid #eaeaea; border-left: 1px solid #eaeaea;\"> " +
                "<div style=\"font-size: 14px; line-height: 18px; color: #444; padding-bottom: 20px;\">" +
                $" <h3> Hi {infor.FullName}, </h3>" +
                $" <div> Dream Store is very happy to receive your order. We will contact you shortly to confirm your order. Your ID order is #{idOrder}" +
                "<br/>" +
                " <h4 style=\"border-top: 1px solid black;\">Order Information </h4> " +
                "<table> ";

            string closeTagTemplate =
                " <div style=\"font-weight: bold; font-size: 15px; margin: 20px;\">Delivery Address</div>" +
                $"<div style=\"margin: 20px;\"> {address}</div>" +
                "</div>" +
                " </div>" +
                " </div>" +
                " </div> " +
                "</div>" +
                " </div>" +
                " </div>";

            string headers =
                "<tr> " +
                    "<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\" >#Id</th> " +
                    "<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Product Name</th> " +
                    "<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Quantity</th> " +
                    "<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Size</th> " +
                    "<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Price</th> " +
                "</tr>";

            string lastRow = "<tr> " +
                        "<td></td> " +
                        "<td></td> " +
                        "<td></td> " +
                        $"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Total Price</td> " +
                        $"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{string.Format("{0:N0}", productCarts.Sum(x => x.TotalVND))} đ - ${string.Format("{0:N0}", productCarts.Sum(x => x.TotalUSD))}</td> " +
                    "</tr>" + "</table>";

            productsInfor.Append(headers);

            foreach (var product in productCarts)
            {
                string newRow =
                    "<tr> " +
                        $"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{product.Id}</td> " +
                        $"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{product.Name}</td> " +
                        $"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{product.SelectedQuantity}</td> " +
                        $"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{(int)product.Size}</td> " +
                        $"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{string.Format("{0:N0}", product.PriceVND)}đ - ${string.Format("{0:N0}", product.PriceUSD)}</td> " +
                    "</tr>";
                productsInfor.Append(newRow);
            }

            BodyBuilder bodyBuilder = new()
            {
                HtmlBody = inforAccountTemplate + productsInfor + lastRow + closeTagTemplate,
                TextBody = "Confirm order from Dreams Store"
            };
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                SmtpClient client = new();
                //connect (smtp address, port , true)
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                //await client.AuthenticateAsync("dreamsstore.ss@gmail.com", "4thanggay");
                await client.AuthenticateAsync("dreamsstore.ss@gmail.com", "pbyqqvmlkutbkavj");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new UserFriendlyException(ErrorCode.NotResponse);
            }
        }

        public async Task<OrderInfor> GetOrderById(int id)
        {
            var orderInfor = await _context.Orders
                                        .Where(x => !x.IsDeleted && x.Id == id)
                                        .Select(x => new OrderInfor
                                        {
                                            Id = x.Id,
                                            Addess = x.Address,
                                            FullName = x.Customer.FullName,
                                            PriceVND = x.OrderDetails.Sum(orderDetail => orderDetail.TotalPrice),
                                            Phone = x.Customer.PhoneNumber
                                        })
                                        .FirstOrDefaultAsync();
            return orderInfor;
        }

        private void ClearCart() => _session.Remove(CART.CART_KEY);

        private void SaveCartSession(List<ProductCartModel> products)
        {
            string jsonCart = JsonConvert.SerializeObject(products);
            _session.SetString(CART.CART_KEY, jsonCart);
        }

        private int QuantityProductCart()
        {
            return GetProductsCart().Sum(x => x.SelectedQuantity);
        }
    }
}
