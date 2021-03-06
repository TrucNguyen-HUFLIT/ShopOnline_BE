using ShopOnline.Core.Models;
using ShopOnline.Core.Models.Account;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopOnline.Business
{
    public interface IUserBusiness
    {
        Task<UserInfor> GetUserInforByClaimAsync(ClaimsPrincipal user);
        UserInfor LoadInforUser(ClaimsPrincipal user);
        Task<AccessTokenModel> LoginAsync(AccountLoginModel accountLogin);

        Task RegisterAsync(AccountRegisterModel accountRegister);

        Task ResetPasswordAsync(ResetPasswordModel resetPasswordModel);
        Task<bool> UpdateProfileAsync(UserInfor userInfor);

        Task<ChangePassword> GetInforUserChangePassword(ClaimsPrincipal user);
        Task<bool> ChangePasswordUser(ChangePassword changePassword);
        Task<UserInforModel> GetUserInforCustomerAsync();
        Task<UserInforModel> UpdateUserInforCustomerAsync(UserInforModel model);
    }
}
