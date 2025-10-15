using FluentValidation;
using EmployeeManager.Application.Department.Commands;

namespace EmployeeManager.Application.Validators.Department
{

    public class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
    {
        public DeleteDepartmentCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1"); 
        }
    }
}
