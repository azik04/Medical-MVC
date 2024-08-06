using FluentValidation;
using MedicalMVC.Models;

namespace MedicalMVC.Validation
{
    public class ContactValidation : AbstractValidator<Request>
    {
        public ContactValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Please enter your email address.")
                .EmailAddress().WithMessage("Please enter a valid email address.");
            RuleFor(x => x.Message).NotEmpty().WithMessage("Please enter your email message.");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Please enter your phone");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Please enter your Name");
        }
    }
}
