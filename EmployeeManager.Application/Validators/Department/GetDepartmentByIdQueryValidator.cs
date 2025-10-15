using FluentValidation;
using EmployeeManager.Application.Company.Queries;

namespace EmployeeManager.Application.Validators.Company
{

    public class GetDepartmentByIdQueryValidator : AbstractValidator<GetDepartmentByIdQuery>
    {
        public GetDepartmentByIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1"); 
        }
    }
}
