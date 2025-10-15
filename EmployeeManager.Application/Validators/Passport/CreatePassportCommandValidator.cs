using EmployeeManager.Application.UseCases.Department.Commands;
using EmployeeManager.Application.UseCases.Passport.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Validators.Passport
{
    public class CreatePassportCommandValidator : AbstractValidator<CreatePassportCommand>
    {
        public CreatePassportCommandValidator()
        {
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
