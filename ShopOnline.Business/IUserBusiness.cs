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
        Task<ClaimsPrincipal> LoginAsync(AccountLoginModel accountLogin);

        Task<bool> RegisterAsync(AccountRegisterModel accountRegister);

        Task ResetPasswordAsync(ResetPasswordModel resetPasswordModel);
        Task<bool> UpdateProfileAsync(UserInfor userInfor);

        Task<ChangePassword> GetInforUserChangePassword(ClaimsPrincipal user);
        Task<bool> ChangePasswordUser(ChangePassword changePassword);
    }
}
