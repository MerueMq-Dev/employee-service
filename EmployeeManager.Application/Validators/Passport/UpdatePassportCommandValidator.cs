using EmployeeManager.Application.Department.Commands;
using EmployeeManager.Application.Passport.Commands;
using FluentValidation;

namespace EmployeeManager.Application.Validators.Passport
{
    public class UpdatePassportCommandValidator : AbstractValidator<UpdatePassportCommand>
    {
        public UpdatePassportCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1");

            RuleFor(c => c.Type)
                .NotNull().WithMessage("Type is required")
                .NotEmpty().WithMessage("Type is required")
                .MaximumLength(50).WithMessage("Type must not exceed 30 characters");


            RuleFor(c => c.Number)
                .NotNull().WithMessage("Number is required")
                .NotEmpty().WithMessage("Number is required")
                .MaximumLength(50).WithMessage("Number must not exceed 30 numbers");

        }
    }
}
