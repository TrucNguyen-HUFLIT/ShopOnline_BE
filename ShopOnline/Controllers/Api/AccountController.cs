using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Models.Account;
using ShopOnline.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;

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
