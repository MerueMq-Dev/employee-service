using EmployeeManager.Application.UseCases.Company.Commands;
using FluentValidation;

namespace EmployeeManager.Application.Validators.Company
{

    public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
    {
        public DeleteCompanyCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("Id can not be less then 1"); 
        }
    }
}
