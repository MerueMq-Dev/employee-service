using EmployeeManager.Application.UseCases.Company.Queries;
using EmployeeManager.Application.UseCases.EmployeeWithDetails.Queries;
using FluentValidation;

namespace EmployeeManager.Application.Validators.Company
{

    public class GetEmployeeWithDetailsByIdQueryValidator : AbstractValidator<GetEmployeeWithDetailsByIdQuery>
    {
        public GetEmployeeWithDetailsByIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1"); 
        }
    }
}
