using FluentValidation;
using MedicalMVC.ViewModel.Categories;

namespace MedicalMVC.Validation
{
    public class CreateCategoryVMValidation : AbstractValidator<CreateCategoryVM>
    {
        public CreateCategoryVMValidation() 
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("Please enter your category name.");
        }
    }
}
