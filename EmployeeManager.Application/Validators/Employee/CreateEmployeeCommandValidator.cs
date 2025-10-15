using EmployeeManager.Application.UseCases.Department.Commands;
using EmployeeManager.Application.UseCases.Employee.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Validators.Employee
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull().WithMessage("Name is required")
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name must not exceed 30 characters");

            RuleFor(c => c.Surname)
                .NotEmpty()
                .NotNull().WithMessage("Surname is required")
                .NotEmpty().WithMessage("Surname is required")
                .MaximumLength(50).WithMessage("Surname must not exceed 30 characters");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .Matches(@"^\+?[0-9\s\-\(\)]{10,20}$")
                .WithMessage("Phone number format is invalid");

            RuleFor(c => c.DepartmentId)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Company Id can not be less then 1");
        }
    }
}
