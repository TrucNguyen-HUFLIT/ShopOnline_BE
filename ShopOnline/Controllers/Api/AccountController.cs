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
        public async Task<IActionResult> Login(AccountLoginModel accountLogin)
        {
            var claimsPrincipal = await _userBusiness.LoginAsync(accountLogin);

            if (claimsPrincipal != null)
            {
                await HttpContext.SignInAsync(claimsPrincipal);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
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

        [HttpPost("Register")]
        public async Task<IActionResult> Register(AccountRegisterModel accountRegister)
        {
            bool isSuccess = await _userBusiness.RegisterAsync(accountRegister);
            if (isSuccess)
            {
                return Created("/Login", new AccountLoginModel
                {
                    Email = accountRegister.Email,
                    Password = accountRegister.Password
                });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword()
        {
            return Ok("ResetPassword");
        }

        [HttpPost("ResetPassword")]
        public async Task ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            await _userBusiness.ResetPasswordAsync(resetPasswordModel);
        }
    }
}
