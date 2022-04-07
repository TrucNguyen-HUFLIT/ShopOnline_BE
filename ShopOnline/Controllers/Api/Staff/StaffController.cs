using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Filters;
using ShopOnline.Core.Models;
using ShopOnline.Core.Models.Staff;
using System;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Controllers.Staff
{
    public class StaffController : ApiController
    {
        private readonly IStaffBusiness _staffBusiness;

        public StaffController(IStaffBusiness staffBusiness)
        {
            _staffBusiness = staffBusiness;
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("ListStaff")]
        public async Task<IActionResult> ListStaff(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            //ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

            if (searchString != null) page = 1;
            else searchString = currentFilter;
            //ViewBag.CurrentFilter = searchString;

            var model = new StaffModel
            {
                ListStaff = await _staffBusiness.GetListStaffAccAsync(sortOrder, TypeAcc.Staff, searchString, page)
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("CreateStaff")]
        public IActionResult CreateStaff()
        {
            var model = new StaffCreateViewModel
            {
                StaffCreate = new StaffCreate(),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("CreateStaff")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateStaff([FromForm] StaffCreate staffCreate)
        {
            await _staffBusiness.CreateAsync(staffCreate, TypeAcc.Staff);
            return Ok();
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("UpdateStaff")]
        public IActionResult UpdateStaff(int id)
        {
            var model = new StaffEditViewModel
            {
                StaffEdit = _staffBusiness.GetStaffById(id, TypeAcc.Staff),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("UpdateStaff")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateStaff(StaffEdit staffEdit)
        {
            await _staffBusiness.EditAsync(staffEdit, TypeAcc.Staff);
            return Ok(staffEdit.Id);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("DeleteStaff")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            await _staffBusiness.DeleteStaffAsync(id, TypeAcc.Staff);
            return Ok();
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("ListShipper")]
        public async Task<IActionResult> ListShipper(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            //ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

            if (searchString != null) page = 1;
            else searchString = currentFilter;
            //ViewBag.CurrentFilter = searchString;

            var model = new StaffModel
            {
                ListStaff = await _staffBusiness.GetListShipperAccAsync(sortOrder, searchString, page)
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("CreateShipper")]
        public IActionResult CreateShipper()
        {
            var model = new StaffCreateViewModel
            {
                StaffCreate = new StaffCreate(),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("CreateShipper")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateShipper([FromForm] StaffCreate staffCreate)
        {
            await _staffBusiness.CreateShipperAsync(staffCreate);
            return Ok();
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("UpdateShipper")]
        public IActionResult UpdateShipper(int id)
        {
            var model = new StaffEditViewModel
            {
                StaffEdit = _staffBusiness.GetShipperById(id),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("UpdateShipper")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateShipper(StaffEdit staffEdit)
        {
            await _staffBusiness.EditShipperAsync(staffEdit);
            return Ok(staffEdit.Id);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("DeleteShipper")]
        public async Task<IActionResult> DeleteShipper(int id)
        {
            await _staffBusiness.DeleteShipperAsync(id);
            return Ok();
        }

        [Authorize(Roles = ROLE.ADMIN)]
        [HttpGet("ListManager")]
        public async Task<IActionResult> ListManager(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            //ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

            if (searchString != null) page = 1;
            else searchString = currentFilter;
            //ViewBag.CurrentFilter = searchString;

            var model = new StaffModel
            {
                ListStaff = await _staffBusiness.GetListStaffAccAsync(sortOrder, TypeAcc.Manager, searchString, page)
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.ADMIN)]
        [HttpGet("CreateManager")]
        public IActionResult CreateManager()
        {
            var model = new StaffCreateViewModel
            {
                StaffCreate = new StaffCreate(),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.ADMIN)]
        [HttpPost("CreateManager")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateManager([FromForm] StaffCreate staffCreate)
        {
            await _staffBusiness.CreateAsync(staffCreate, TypeAcc.Manager);
            return Ok();
        }

        [Authorize(Roles = ROLE.ADMIN)]
        [HttpGet("UpdateManager")]
        public IActionResult UpdateManager(int id)
        {
            var model = new StaffEditViewModel
            {
                StaffEdit = _staffBusiness.GetStaffById(id, TypeAcc.Manager),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.ADMIN)]
        [HttpPost("UpdateManager")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateManager(StaffEdit staffEdit)
        {
            await _staffBusiness.EditAsync(staffEdit, TypeAcc.Manager);
            return Ok(staffEdit.Id);
        }

        [Authorize(Roles = ROLE.ADMIN)]
        [HttpGet("DeleteManager")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            await _staffBusiness.DeleteStaffAsync(id, TypeAcc.Manager);
            return Ok();
        }
    }
}
