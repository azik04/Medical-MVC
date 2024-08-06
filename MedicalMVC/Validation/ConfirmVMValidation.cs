using FluentValidation;
using MedicalMVC.ViewModel.Products;

namespace MedicalMVC.Validation
{
    public class ConfirmVMValidation : AbstractValidator<ConfirmViewModel>
    {
        public ConfirmVMValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter your name.");
            RuleFor(x => x.AgeGroup).NotEmpty().WithMessage("Please enter your age group.");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Please enter your country.");
            RuleFor(x => x.Control).NotEmpty().WithMessage("Please enter your control.");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Please enter your type.");
            RuleFor(x => x.Brand).NotEmpty().WithMessage("Please enter your brand.");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Please enter your name.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter your description.");
            RuleFor(x => x.UserPhone).NotEmpty().WithMessage("Please enter your phone number.");
            RuleFor(x => x.UserEmail).NotEmpty().WithMessage("Please enter your email address.")
                .EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(x => x.Owners).NotEmpty().WithMessage("Please enter the number of product owners.");
        }
    }
}
