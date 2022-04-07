using ShopOnline.Core.Models.Staff;
using System.Threading.Tasks;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Business.Staff
{
    public interface IStaffBusiness
    {
        Task<IPagedList<StaffInfor>> GetListStaffAccAsync(string sortOrder, TypeAcc typeAcc, string searchString, int? page);
        Task CreateAsync(StaffCreate staffCreate, TypeAcc typeAcc);
        StaffEdit GetStaffById(int id, TypeAcc typeAcc);
        Task<bool> EditAsync(StaffEdit staffEdit, TypeAcc typeAcc);
        Task<bool> DeleteStaffAsync(int id, TypeAcc typeAcc);
        Task<IPagedList<StaffInfor>> GetListShipperAccAsync(string sortOrder, string searchString, int? page);
        Task CreateShipperAsync(StaffCreate staffCreate);
        StaffEdit GetShipperById(int id);
        Task<bool> EditShipperAsync(StaffEdit staffEdit);
        StaffEdit GetShipperByEmail(string email);
        Task<bool> DeleteShipperAsync(int id);
    }
}
