using FluentValidation;
using ShopOnline.Core.Models.Account;

namespace ShopOnline.Core.Validators.Account
{
    public class AccountRegisterValidator : AbstractValidator<AccountRegisterModel>
    {
        public AccountRegisterValidator()
        {
            RuleFor(actor => actor.Email).NotEmpty();
            RuleFor(actor => actor.FullName).NotEmpty();
            RuleFor(actor => actor.PhoneNumber).NotEmpty();
            RuleFor(actor => actor.Password).NotEmpty();
            RuleFor(actor => actor.ConfirmPassword).NotEmpty();
            RuleFor(actor => actor.ConfirmPassword).Equal(actor => actor.Password).WithMessage("Confirm password didn't match");
        }
    }
}
