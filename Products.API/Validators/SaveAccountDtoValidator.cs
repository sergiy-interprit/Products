using FluentValidation;
using Products.API.Models;

namespace Products.API.Validations
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