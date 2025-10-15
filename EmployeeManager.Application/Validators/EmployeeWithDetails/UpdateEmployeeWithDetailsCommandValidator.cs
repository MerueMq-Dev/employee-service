using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.UseCases.EmployeeWithDetails.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Validators.EmployeeWithDetails
{
    public class UpdateEmployeeWithDetailsCommandValidator
    : AbstractValidator<UpdateEmployeeWithDetailsCommand>
    {
        public UpdateEmployeeWithDetailsCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than zero");

            RuleFor(x => x.Name)
                .MaximumLength(25).WithMessage("Name cannot exceed 25 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Surname)
                .MaximumLength(25).WithMessage("Surname cannot exceed 25 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Surname));

            RuleFor(x => x.Phone)
                .Matches(@"^\+?[0-9\s\-\(\)]{10,20}$")
                .WithMessage("Phone number format is invalid")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone));

            RuleFor(x => x.Department)
                .SetValidator(new UpdateDepartmentDetailsDtoValidator()!)
                .When(x => x.Department is not null);

            RuleFor(x => x.Passport)
                .SetValidator(new UpdatePassportDetailsDtoValidator()!)
                .When(x => x.Passport is not null);
        }
    }


    public class UpdateDepartmentDetailsDtoValidator : AbstractValidator<DepartmentDetailsDto>
    {
        public UpdateDepartmentDetailsDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Department name is required")
                .MaximumLength(100)
                .WithMessage("Department Name cannot exceed 100 characters")
                .When(x => x is not null); ;

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Department phone is required")
                .Matches(@"^\+?[0-9\s\-\(\)]{10,20}$")
                .WithMessage("Phone number format is invalid")
                .When(x => x is not null);
        }
    }

    public class UpdatePassportDetailsDtoValidator : AbstractValidator<PassportDetailsDto>
    {
        public UpdatePassportDetailsDtoValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Passport type is required")
                .MaximumLength(30)
                .WithMessage("Passport type cannot exceed 30 characters")
                .When(x => x is not null);

            RuleFor(x => x.Number)
                .NotEmpty().WithMessage("Passport number is required")
                .MaximumLength(100)
                .WithMessage("Passport type cannot exceed 100 symbols")
                .When(x => x is not null);
        }
    }
}
