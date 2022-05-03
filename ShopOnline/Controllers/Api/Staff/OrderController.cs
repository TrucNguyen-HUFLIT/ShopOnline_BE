using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Filters;
using ShopOnline.Core.Models;
using ShopOnline.Core.Models.HistoryOrder;
using ShopOnline.Core.Models.Order;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Controllers.Staff
{
    public class OrderController : ApiController
    {
        private readonly IOrderBusiness _orderBusiness;
        private readonly IReviewBusiness _reviewBusiness;

        public OrderController(IOrderBusiness orderBusiness, IReviewBusiness reviewBusiness)
        {
            _orderBusiness = orderBusiness;
            _reviewBusiness = reviewBusiness;
        }

        [AuthorizeFilter(TypeAcc.Staff)]
        [HttpGet("ListOrder")]
        public async Task<IActionResult> ListOrder(string sortOrder, int statusOrder, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.OrderDay = String.IsNullOrEmpty(sortOrder) ? "order_day_desc" : "";

            var enumStatusOrder = (StatusOrder)statusOrder;

            var model = new OrderModel
            {
                ListCustomer = await _reviewBusiness.GetListCustomer(),
                ListOrder = await _orderBusiness.GetListOrderAsync(sortOrder, enumStatusOrder, page)
            };

            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Staff)]
        [HttpGet("ListOrderProcessing")]
        public async Task<IActionResult> ListOrderProcessing(string sortOrder, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.OrderDay = String.IsNullOrEmpty(sortOrder) ? "order_day" : "";

            var model = new OrderModel
            {
                ListCustomer = await _reviewBusiness.GetListCustomer(),
                ListOrder = await _orderBusiness.GetListOrderAsync(sortOrder, StatusOrder.Processing, page)
            };

            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Staff)]
        [HttpGet("AcceptOrder")]
        public async Task<IActionResult> AcceptOrder(int id)
        {
            await _orderBusiness.StaffChangeStatusOrderAsync(id, StatusOrder.Accepted);
            return RedirectToAction("ListOrderProcessing");
        }

        [AuthorizeFilter(TypeAcc.Staff)]
        [HttpGet("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            await _orderBusiness.StaffChangeStatusOrderAsync(id, StatusOrder.Cancelled);
            return RedirectToAction("ListOrderProcessing");
        }

        [AuthorizeFilter(TypeAcc.Staff)]
        [HttpGet("PayOrder")]
        public async Task<IActionResult> PayOrder(int id, bool isPaid)
        {
            await _orderBusiness.SetIsPaidOrderAsync(id, isPaid);
            return RedirectToAction("ListOrderProcessing");
        }

        [AuthorizeFilter(TypeAcc.Shipper)]
        [HttpGet("ListHistoryOrderShipper")]
        public async Task<IActionResult> ListHistoryOrderShipperAsync(string sortOrder, string currentFilter, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.OrderDay = String.IsNullOrEmpty(sortOrder) ? "order_day" : "";

            var model = new HistoryOrderShipperModel
            {
                ListCustomer = await _reviewBusiness.GetListCustomer(),
                ListHistoryOrderShipper = await _orderBusiness.GetHistoryOrderShipperAsync(sortOrder, currentFilter, page, User)
            };

            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Shipper)]
        [HttpGet("ListOrderShipper")]
        public async Task<IActionResult> ListOrderShipperAsync(string sortOrder, string currentFilter, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.OrderDay = String.IsNullOrEmpty(sortOrder) ? "order_day" : "";

            var model = new OrderInforShipperModel
            {
                ListCustomer = await _reviewBusiness.GetListCustomer(),
                ListOrderInforShipper = await _orderBusiness.GetOrderAcceptedShipperAsync(sortOrder, currentFilter, page)
            };

            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Shipper)]
        [HttpGet("AcceptDelivery")]
        public async Task<IActionResult> AcceptDelivery(int id)
        {
            await _orderBusiness.ShipperChangeStatusOrderAsync(id, StatusOrder.Delivering, User);
            return RedirectToAction("ListOrderShipper");
        }

        [AuthorizeFilter(TypeAcc.Shipper)]
        [HttpGet("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            await _orderBusiness.ShipperChangeStatusOrderAsync(id, StatusOrder.Completed, User);
            return RedirectToAction("ListOrderShipper");
        }
    }
}
