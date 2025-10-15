using EmployeeManager.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Validators.Company
{
   
    public class CompanyDtoValidator : AbstractValidator<CompanyDto>
    {
        public CompanyDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("Name is required")
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(25).WithMessage("Name must not exceed 25 characters");

                RuleFor(c => c.Address)
                    .NotNull().WithMessage("Address is required")
                    .NotEmpty().WithMessage("Addressis required")
                    .MaximumLength(200).WithMessage("Address must not exceed 200 characters");

        }       
    }
}
