using FluentValidation;
using Products.API.Dto;

namespace Products.Services.Validators
{
    public class SaveProductDtoValidator : AbstractValidator<SaveProductDto>
    {
        public SaveProductDtoValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .MaximumLength(50);
            
            RuleFor(m => m.AccountId)
                .NotEmpty()
                .WithMessage("'Account Id' must not be 0.");
        }
    }
}