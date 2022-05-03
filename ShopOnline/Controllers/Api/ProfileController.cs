using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Filters;
using ShopOnline.Core.Models;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class ProfileController : ApiController
    {
        private readonly IUserBusiness _userBusiness;

        public ProfileController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        [HttpGet("UpdateDetail")]
        public async Task<IActionResult> UpdateDetailAsync()
        {
            var model = new ProfileViewModel
            {
                UserInfor = await _userBusiness.GetUserInforByClaimAsync(User)
            };

            if (model.UserInfor != null)
                return Ok(model);
            else
                return NotFound();
        }

        [HttpPost("UpdateDetail")]
        public async Task<IActionResult> UpdateDetailAsync(UserInfor userInfor)
        {
            await _userBusiness.UpdateProfileAsync(userInfor);
            return RedirectToAction("UpdateDetail");
        }

        [HttpGet("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync()
        {
            var model = new ChangePasswordViewModel
            {
                ChangePassword = await _userBusiness.GetInforUserChangePassword(User)
            };
            if (model.ChangePassword != null)
                return Ok(model);
            else
                return NotFound();
        }

        [HttpPost("ChangePassword")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> ChangePasswordAsync(ChangePassword changePassword)
        {
            await _userBusiness.ChangePasswordUser(changePassword);
            return Ok();
        }
    }
}
