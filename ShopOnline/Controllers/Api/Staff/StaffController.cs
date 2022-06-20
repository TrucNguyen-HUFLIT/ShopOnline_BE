using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Filters;
using ShopOnline.Core.Models.Staff;
using ShopOnline.Core.Validators.Paging;
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

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpGet("")]
        public async Task<PagedCollectionResultModel<StaffInfor>> ListStaff([FromQuery] StaffParamsModel model)
        {
            var staffs = await _staffBusiness.GetListStaffAsync(model, TypeAcc.Staff);
            return staffs;
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpGet("CreateStaff")]
        public IActionResult CreateStaff()
        {
            var model = new StaffCreateViewModel
            {
                StaffCreate = new StaffCreate(),
            };
            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpPost("create-staff")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateStaff([FromBody] StaffCreate staffCreate)
        {
            var staff = await _staffBusiness.CreateAsync(staffCreate, TypeAcc.Staff);
            return Ok(staff);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [AllowAnonymous]
        [HttpGet("get-staff/{id}")]
        public async Task<IActionResult> GetStaffById(int id)
        {
            var model = await _staffBusiness.GetStaffById(id, TypeAcc.Staff);
            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpPut("update-staff")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateStaff(StaffEdit staffEdit)
        {
            await _staffBusiness.EditAsync(staffEdit, TypeAcc.Staff);
            return Ok(staffEdit.Id);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpGet("DeleteStaff")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            await _staffBusiness.DeleteStaffAsync(id, TypeAcc.Staff);
            return Ok();
        }

        [AuthorizeFilter(TypeAcc.Manager)]
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

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpGet("CreateShipper")]
        public IActionResult CreateShipper()
        {
            var model = new StaffCreateViewModel
            {
                StaffCreate = new StaffCreate(),
            };
            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpPost("CreateShipper")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateShipper([FromForm] StaffCreate staffCreate)
        {
            await _staffBusiness.CreateShipperAsync(staffCreate);
            return Ok();
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpGet("UpdateShipper")]
        public IActionResult UpdateShipper(int id)
        {
            var model = new StaffEditViewModel
            {
                StaffEdit = _staffBusiness.GetShipperById(id),
            };
            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpPost("UpdateShipper")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateShipper(StaffEdit staffEdit)
        {
            await _staffBusiness.EditShipperAsync(staffEdit);
            return Ok(staffEdit.Id);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpGet("DeleteShipper")]
        public async Task<IActionResult> DeleteShipper(int id)
        {
            await _staffBusiness.DeleteShipperAsync(id);
            return Ok();
        }

        //[AuthorizeFilter(TypeAcc.Admin)]
        //[HttpGet("ListManager")]
        //public async Task<IActionResult> ListManager(string sortOrder, string currentFilter, string searchString, int? page)
        //{
        //    //ViewBag.CurrentSort = sortOrder;
        //    //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

        //    //ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

        //    if (searchString != null) page = 1;
        //    else searchString = currentFilter;
        //    //ViewBag.CurrentFilter = searchString;

        //    var model = new StaffModel
        //    {
        //        ListStaff = await _staffBusiness.GetListStaffAccAsync(sortOrder, TypeAcc.Manager, searchString, page)
        //    };
        //    return Ok(model);
        //}

        [AuthorizeFilter(TypeAcc.Admin)]
        [HttpGet("CreateManager")]
        public IActionResult CreateManager()
        {
            var model = new StaffCreateViewModel
            {
                StaffCreate = new StaffCreate(),
            };
            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Admin)]
        [HttpPost("CreateManager")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateManager([FromForm] StaffCreate staffCreate)
        {
            await _staffBusiness.CreateAsync(staffCreate, TypeAcc.Manager);
            return Ok();
        }

        //[AuthorizeFilter(TypeAcc.Admin)]
        //[HttpGet("UpdateManager")]
        //public IActionResult UpdateManager(int id)
        //{
        //    var model = new StaffEditViewModel
        //    {
        //        StaffEdit = aww _staffBusiness.GetStaffById(id, TypeAcc.Manager),
        //    };
        //    return Ok(model);
        //}

        [AuthorizeFilter(TypeAcc.Admin)]
        [HttpPost("UpdateManager")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateManager(StaffEdit staffEdit)
        {
            await _staffBusiness.EditAsync(staffEdit, TypeAcc.Manager);
            return Ok(staffEdit.Id);
        }

        [AuthorizeFilter(TypeAcc.Admin)]
        [HttpGet("DeleteManager")]
        public async Task<IActionResult> DeleteManager(int id)
        {
            await _staffBusiness.DeleteStaffAsync(id, TypeAcc.Manager);
            return Ok();
        }
    }
}
