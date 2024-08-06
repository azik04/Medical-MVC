using FluentValidation;
using MedicalMVC.ViewModel.Accounts;

namespace MedicalMVC.Validation
{
    public class AccountVMValidation : AbstractValidator<AccountVM>
    {
        public AccountVMValidation()
        {
            RuleFor(x=>x.UserName).NotEmpty().WithMessage("Please enter your name.");
            RuleFor(x => x.UserPassword).NotEmpty().WithMessage("Please enter your password.");
        }
    }
}
