using FluentValidation;
using ShopOnline.Core.Models.Account;

namespace ShopOnline.Core.Validators.Account
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordValidator()
        {
            RuleFor(actor => actor.Email).NotEmpty();
            RuleFor(actor => actor.PhoneNumber).NotEmpty();
        }
    }
}
