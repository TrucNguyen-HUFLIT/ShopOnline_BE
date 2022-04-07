using ShopOnline.Core.Models.HistoryOrder;
using ShopOnline.Core.Models.Order;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Business.Staff
{
    public interface IOrderBusiness
    {
        Task<IPagedList<OrderInfor>> GetListOrderAsync(string sortOrder, StatusOrder statusOrder, int? page);
        Task<IPagedList<HistoryOrderInfor>> GetHistoryOrderCustomerAsync(string sortOrder, string currentFilter, int? page, ClaimsPrincipal user);
        Task<IPagedList<HistoryOrderShipperInfor>> GetHistoryOrderShipperAsync(string sortOrder, string currentFilter, int? page, ClaimsPrincipal user);
        Task<IPagedList<OrderInforShipper>> GetOrderAcceptedShipperAsync(string sortOrder, string currentFilter, int? page);
        Task ShipperChangeStatusOrderAsync(int id, StatusOrder statusOrder, ClaimsPrincipal user);
        Task StaffChangeStatusOrderAsync(int id, StatusOrder statusOrder);
        Task SetIsPaidOrderAsync(int id, bool isPaid);
    }
}
