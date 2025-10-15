using FluentValidation;
using EmployeeManager.Application.Company.Queries;
using EmployeeManager.Application.UseCases.Passport.Queries;

namespace EmployeeManager.Application.Validators.Passport
{

    public class GetPassportByIdQueryValidator : AbstractValidator<GetPassportByIdQuery>
    {
        public GetPassportByIdQueryValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1"); 
        }
    }
}
