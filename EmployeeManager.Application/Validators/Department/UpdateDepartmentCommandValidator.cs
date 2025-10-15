using EmployeeManager.Application.Department.Commands;
using FluentValidation;

namespace EmployeeManager.Application.Validators.Department
{
    public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
    {
        public UpdateDepartmentCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1");


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
