using FluentValidation;
using Products.API.Dto;

namespace Products.Services.Validators
{
    public class SaveAccountDtoValidator : AbstractValidator<SaveAccountDto>
    {
        public SaveAccountDtoValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(a => a.Description)
                .MaximumLength(200);
        }
    }
}