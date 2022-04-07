using FluentValidation;
using ShopOnline.Core.Models.Staff;

namespace ShopOnline.Core.Validators.Staff
{
    public class StaffEditValidator : AbstractValidator<StaffEdit>
    {
        public StaffEditValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
        }
    }
}
