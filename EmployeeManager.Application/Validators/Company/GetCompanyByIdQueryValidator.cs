using EmployeeManager.Application.UseCases.Company.Queries;
using FluentValidation;

namespace EmployeeManager.Application.Validators.Company
{

    public class GetCompanyByIdQueryValidator : AbstractValidator<GetCompanyByIdQuery>
    {
        public GetCompanyByIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1"); 
        }
    }
}
