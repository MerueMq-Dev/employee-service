using EmployeeManager.Application.UseCases.EmployeeWithDetails.Queries;
using FluentValidation;

namespace EmployeeManager.Application.Validators.Company
{

    public class GetEmployeeWithDetailsByDepartmentIdQueryValidator : AbstractValidator<GetEmployeeWithDetailsByDepartmentIdQuery>
    {
        public GetEmployeeWithDetailsByDepartmentIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1"); 
        }
    }
}
