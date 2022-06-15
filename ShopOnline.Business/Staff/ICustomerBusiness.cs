using ShopOnline.Core.Models.Customer;
using ShopOnline.Core.Validators.Paging;
using System.Threading.Tasks;
using X.PagedList;

namespace ShopOnline.Business.Staff
{
    public interface ICustomerBusiness
    {
        Task<PagedCollectionResultModel<CustomerInfor>> GetListCustomerAsync(CustomerParamsModel model);
    }
}
