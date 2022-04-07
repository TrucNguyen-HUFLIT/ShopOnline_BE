using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.Staff
{
    public class StaffModel
    {
        public StaffInfor staffInfor { get; set; }
        public IPagedList<StaffInfor> ListStaff { get; set; }
    }

    public class StaffCreateViewModel
    {
        public StaffCreate StaffCreate { get; set; }
    }
    public class StaffEditViewModel
    {
        public StaffEdit StaffEdit { get; set; }
    }

    public class StaffInfor
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public TypeAcc TypeAcc { get; set; }
    }
    public class StaffCreate
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public TypeAcc TypeAcc { get; set; }
        public string Password { get; set; }
        public int Salary { get; set; }

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public IFormFile UploadAvt { get; set; }
    }
    public class StaffEdit : StaffInfor
    {
        [NotMapped]
        public IFormFile UploadAvt { get; set; }
    }
}
