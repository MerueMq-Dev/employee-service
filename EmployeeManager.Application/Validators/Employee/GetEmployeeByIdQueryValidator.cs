using EmployeeManager.Application.Company.Queries;
using EmployeeManager.Application.Employee.Queries;
using FluentValidation;

namespace EmployeeManager.Application.Validators.Employee
{
    public class GetEmployeeByIdQueryValidator : AbstractValidator<GetEmployeeByIdQuery>
    {
        public GetEmployeeByIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1"); 
        }
    }
}
