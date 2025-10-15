using FluentValidation;
using EmployeeManager.Application.Passport.Commands;

namespace EmployeeManager.Application.Validators.Passport
{

    public class DeletePassportCommandValidator : AbstractValidator<DeletePassportCommand>
    {
        public DeletePassportCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1"); 
        }
    }
}
