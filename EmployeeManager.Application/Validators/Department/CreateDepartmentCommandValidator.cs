using EmployeeManager.Application.UseCases.Department.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Validators.Department
{
    public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
    {
        public CreateDepartmentCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull().WithMessage("Name is required")
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters");


            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .Matches(@"^\+?[0-9\s\-\(\)]{10,20}$")
                .WithMessage("Phone number format is invalid");

            RuleFor(c => c.CompanyId)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Company Id can not be less then 1");
        }
    }
}
