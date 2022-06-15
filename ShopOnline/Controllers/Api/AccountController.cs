using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Models.Account;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IUserBusiness _userBusiness;
        public AccountController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return Ok("Login");
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<AccessTokenModel> Login(AccountLoginModel accountLogin)
        {
            return await _userBusiness.LoginAsync(accountLogin);
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Login");
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return Ok("Register");
        }

        [HttpPost("register")]
        public async Task Register(AccountRegisterModel accountRegister)
        {
            await _userBusiness.RegisterAsync(accountRegister);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            await _userBusiness.ResetPasswordAsync(resetPasswordModel);
        }
    }
}
