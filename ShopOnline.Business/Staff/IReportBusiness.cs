using ShopOnline.Core.Models.Report;
using System.Threading.Tasks;
using X.PagedList;

namespace ShopOnline.Business.Staff
{
    public interface IReportBusiness
    {
        Task<IPagedList<ReportModel>> GetListReportAsync(int? page);
    }
}
