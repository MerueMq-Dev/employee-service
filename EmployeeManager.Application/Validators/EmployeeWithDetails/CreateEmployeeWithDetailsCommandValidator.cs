using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.UseCases.EmployeeWithDetails.Commands;
using FluentValidation;

namespace EmployeeManager.Application.Validators.EmployeeWithDetails
{
    public class CreateEmployeeWithDetailsCommandValidator
        : AbstractValidator<CreateEmployeeWithDetailsCommand>
    {
        public CreateEmployeeWithDetailsCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Employee name is required")
                .MaximumLength(25).WithMessage("Employee name must not exceed 25 characters"); ;

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Employee surname is required")
                .MaximumLength(25).WithMessage("Employee surname must not exceed 50 characters"); ;

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Employee phone is required")
                .Matches(@"^\+?[0-9\s\-\(\)]{10,20}$")
                .WithMessage("Phone number format is invalid");

            RuleFor(x => x.CompanyId)
                .GreaterThan(0).WithMessage("CompanyId must be greater than zero");

            RuleFor(x => x.Department)
                .NotNull().WithMessage("Department is required")
                .SetValidator(new DepartmentDetailsDtoValidator());

            RuleFor(x => x.Passport)
                .NotNull().WithMessage("Passport is required")
                .SetValidator(new PassportDetailsDtoValidator());
        }
    }


    public class DepartmentDetailsDtoValidator : AbstractValidator<DepartmentDetailsDto>
    {
        public DepartmentDetailsDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Department phone is required")
                .Matches(@"^\+?[0-9\s\-\(\)]{10,20}$")
                .WithMessage("Phone number format is invalid");
        }
    }

    public class PassportDetailsDtoValidator : AbstractValidator<PassportDetailsDto>
    {
        public PassportDetailsDtoValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Passport type is required")
                .MaximumLength(30);

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Passport number is required")
                .MaximumLength(100);
        }
    }
}
