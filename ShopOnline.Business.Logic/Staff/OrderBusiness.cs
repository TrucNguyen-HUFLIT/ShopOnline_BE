using Microsoft.EntityFrameworkCore;
using ShopOnline.Business.Staff;
using ShopOnline.Core;
using ShopOnline.Core.Models.Enum;
using ShopOnline.Core.Models.HistoryOrder;
using ShopOnline.Core.Models.Order;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Business.Logic.Staff
{
    public class OrderBusiness : IOrderBusiness
    {
        private readonly MyDbContext _context;
        public OrderBusiness(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IPagedList<OrderInfor>> GetListOrderAsync(string sortOrder, StatusOrder statusOrder, int? page)
        {
            var ordersQuery = _context.Orders.Where(x => !x.IsDeleted);

            ordersQuery = sortOrder switch
            {
                "order_day" => ordersQuery.OrderBy(x => x.OrderDay),
                _ => ordersQuery.OrderByDescending(x => x.Id),
            };

            if (statusOrder != 0)
            {
                ordersQuery = ordersQuery.Where(x => x.StatusOrder == statusOrder);
            }
            else
            {
                ordersQuery = ordersQuery.Where(x => x.StatusOrder != StatusOrder.Processing);
            }

            var listOrder = await ordersQuery
              .Select(order => new OrderInfor
              {
                  Id = order.Id,
                  OrderDay = order.OrderDay,
                  Address = order.Address,
                  ExtraFee = order.ExtraFee,
                  StatusOrder = order.StatusOrder,
                  Payment = order.Payment,
                  IsPaid = order.IsPaid,
                  IdCustomer = order.IdCustomer,
                  TotalPrice = order.OrderDetails.Sum(y => y.TotalPrice)
              })
              .ToListAsync();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return listOrder.ToPagedList(pageNumber, pageSize);
        }

        public async Task<IPagedList<HistoryOrderInfor>> GetHistoryOrderCustomerAsync(string sortOrder, string currentFilter, int? page, ClaimsPrincipal user)
        {
            string email = user.FindFirst(ClaimTypes.Email).Value;
            var customerId = _context.Customers.Where(x => x.Email == email && !x.IsDeleted).Select(x => x.Id).FirstOrDefault();

            var listHistoryOrderQuery = _context.Orders.Where(x => !x.IsDeleted && x.IdCustomer == customerId);

            listHistoryOrderQuery = sortOrder switch
            {
                "order_day" => listHistoryOrderQuery.OrderBy(x => x.OrderDay),
                _ => listHistoryOrderQuery.OrderByDescending(x => x.OrderDay),
            };

            var listHistoryOrder = await listHistoryOrderQuery
                .Select(historyOrder => new HistoryOrderInfor
                {
                    Id = historyOrder.Id,
                    OrderDay = historyOrder.OrderDay,
                    Address = historyOrder.Address,
                    ExtraFee = historyOrder.ExtraFee,
                    StatusOrder = historyOrder.StatusOrder,
                    IsPaid = historyOrder.IsPaid,
                    Payment = historyOrder.Payment,
                    TotalPrice = historyOrder.OrderDetails.Sum(y => y.TotalPrice)
                })
                .ToListAsync();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return listHistoryOrder.ToPagedList(pageNumber, pageSize);
        }

        public async Task<IPagedList<HistoryOrderShipperInfor>> GetHistoryOrderShipperAsync(string sortOrder, string currentFilter, int? page, ClaimsPrincipal user)
        {
            string email = user.FindFirst(ClaimTypes.Email).Value;
            var shipperId = _context.Shippers.Where(x => x.Email == email && !x.IsDeleted).Select(x => x.Id).FirstOrDefault();

            var listHistoryOrderQuery = _context.Orders.Where(x => !x.IsDeleted && x.IdShipper == shipperId);

            listHistoryOrderQuery = sortOrder switch
            {
                "order_day" => listHistoryOrderQuery.OrderBy(x => x.OrderDay),
                _ => listHistoryOrderQuery.OrderByDescending(x => x.OrderDay),
            };

            var listHistoryOrder = await listHistoryOrderQuery
                .Select(historyOrder => new HistoryOrderShipperInfor
                {
                    Id = historyOrder.Id,
                    OrderDay = historyOrder.OrderDay,
                    Address = historyOrder.Address,
                    ExtraFee = historyOrder.ExtraFee,
                    StatusOrder = historyOrder.StatusOrder,
                    IdCustomer = historyOrder.IdCustomer,
                    IsPaid = historyOrder.IsPaid,
                    Payment = historyOrder.Payment,
                    TotalPrice = historyOrder.OrderDetails.Sum(y => y.TotalPrice)
                })
                .ToListAsync();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return listHistoryOrder.ToPagedList(pageNumber, pageSize);
        }

        public async Task<IPagedList<OrderInforShipper>> GetOrderAcceptedShipperAsync(string sortOrder, string currentFilter, int? page)
        {
            var ordersAcceptedShipperQuery = _context.Orders.Where(x => !x.IsDeleted && x.StatusOrder == AppEnum.StatusOrder.Accepted);
            ordersAcceptedShipperQuery = sortOrder switch
            {
                "order_day" => ordersAcceptedShipperQuery.OrderBy(x => x.OrderDay),
                _ => ordersAcceptedShipperQuery.OrderByDescending(x => x.OrderDay),
            };

            var listOrderAcceptedShipper = await ordersAcceptedShipperQuery
                .Select(orderShipper => new OrderInforShipper
                {
                    Id = orderShipper.Id,
                    OrderDay = orderShipper.OrderDay,
                    Address = orderShipper.Address,
                    ExtraFee = orderShipper.ExtraFee,
                    StatusOrder = orderShipper.StatusOrder,
                    IdCustomer = orderShipper.IdCustomer,
                    IsPaid = orderShipper.IsPaid,
                    Payment = orderShipper.Payment,
                    TotalPrice = orderShipper.OrderDetails.Sum(y => y.TotalPrice)
                })
                .ToListAsync();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return listOrderAcceptedShipper.ToPagedList(pageNumber, pageSize);
        }

        public async Task ShipperChangeStatusOrderAsync(int id, StatusOrder statusOrder, ClaimsPrincipal user)
        {
            string email = user.FindFirst(ClaimTypes.Email).Value;
            var idShipper = _context.Shippers.Where(x => x.Email == email && !x.IsDeleted).Select(x => x.Id).FirstOrDefault();

            var order = await _context.Orders.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            order.IdShipper = idShipper;
            order.StatusOrder = statusOrder;
            if (statusOrder == StatusOrder.Completed && order.Payment == PaymentMethod.ShipCod)
            {
                order.IsPaid = true;
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task StaffChangeStatusOrderAsync(int id, StatusOrder statusOrder)
        {
            var order = await _context.Orders.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (statusOrder == StatusOrder.Cancelled)
            {
                await RevertQuantityByOrderId(id);
            }

            order.StatusOrder = statusOrder;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task SetIsPaidOrderAsync(int id, bool isPaid)
        {
            var order = await _context.Orders.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            order.IsPaid = isPaid;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        private async Task RevertQuantityByOrderId(int orderId)
        {
            var productOrderDetails = await _context.OrderDetails.Where(x => x.IdOrder == orderId).Select(x => new { x.Product, x.QuantityPurchased }).ToListAsync();
            foreach (var productOrderDetail in productOrderDetails)
            {
                productOrderDetail.Product.Quantity += productOrderDetail.QuantityPurchased;
            }
            _context.UpdateRange(productOrderDetails.Select(x => x.Product).ToArray());
            await _context.SaveChangesAsync();
        }
    }
}
