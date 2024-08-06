using FluentValidation;
using MedicalMVC.ViewModel.Products;

namespace MedicalMVC.Validation
{
    public class RequestViewModelValidator : AbstractValidator<RequestViewModel>
    {
        public RequestViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter your name.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Please enter your description.");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Please enter your phone number.");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter your email address.")
                .EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(x => x.Owners).NotEmpty().WithMessage("Please enter the number of product owners.");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Please select a category.");
            RuleFor(x => x.Photos).NotEmpty().WithMessage("Please select a photo.");
        }
    }
}
