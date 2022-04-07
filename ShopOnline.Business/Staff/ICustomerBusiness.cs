using ShopOnline.Core.Models.Customer;
using System.Threading.Tasks;
using X.PagedList;

namespace ShopOnline.Business.Staff
{
    public interface ICustomerBusiness
    {
        Task<IPagedList<CustomerInfor>> GetListCustomerAsync(string sortOrder, string currentFilter, string searchString, int? page);
    }
}
