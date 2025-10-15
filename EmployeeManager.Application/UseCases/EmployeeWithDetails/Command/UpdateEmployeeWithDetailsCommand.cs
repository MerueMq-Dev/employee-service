using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.UseCases.EmployeeWithDetails.Command
{
    public record UpdateEmployeeWithDetailsCommand : IRequest<EmployeeWithDetailsDto>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public DepartmentDetailsDto? Department { get; set; }
        public PassportDetailsDto? Passport { get; set; }
    }

    public class UpdateEmployeeWithDetailsHandler(
    IValidator<UpdateEmployeeWithDetailsCommand> validator,
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    IPassportRepository passportRepository
    )
    : IRequestHandler<UpdateEmployeeWithDetailsCommand, EmployeeWithDetailsDto>
    {
        public async Task<EmployeeWithDetailsDto> Handle(
            UpdateEmployeeWithDetailsCommand command,
            CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command);

            EmployeeEntity? employee = await employeeRepository.GetByIdAsync(command.Id, cancellationToken);
            if (employee is null)
                throw new NotFoundException($"Employee with id {command.Id} does not exist");
            
            if (!string.IsNullOrWhiteSpace(command.Name))
                employee.Name = command.Name;

            if (!string.IsNullOrWhiteSpace(command.Surname))
                employee.Surname = command.Surname;

            if (!string.IsNullOrWhiteSpace(command.Phone))
                employee.Phone = command.Phone;

            DepartmentEntity? department;
            if (command.Department != null)
            {
                department = await departmentRepository.GetByNameAsync(command.Department.Name, cancellationToken);
                if (department is null)
                    throw new NotFoundException($"Department with name '{command.Department.Name}' does not exist");

                employee.DepartmentId = department.Id;
            }
            else
            {
                department = await departmentRepository.GetByIdAsync(employee.DepartmentId, cancellationToken);
            }

            // 4. Обновляем Passport (если передан)
            PassportEntity? passport;
            if (command.Passport != null)
            {
                passport = await passportRepository.GetByNumberAsync(command.Passport.Number);
                if (passport is null)
                    throw new NotFoundException($"Passport with number '{command.Passport.Number}' does not exist");

               
                EmployeeEntity? employeeWithPassport = await employeeRepository.GetByPassportIdAsync(passport.Id, cancellationToken);
                if (employeeWithPassport is not null && employeeWithPassport.Id != command.Id)
                    throw new BusinessException($"Passport with number '{command.Passport.Number}' is already used by employee {employeeWithPassport.Id}");

                employee.PassportId = passport.Id;
            }
            else
            {
                passport = employee.PassportId.HasValue
                    ? await passportRepository.GetByIdAsync(employee.PassportId.Value, cancellationToken)
                    : null;
            }

            await employeeRepository.UpdateAsync(employee, cancellationToken);

            var companyId = await departmentRepository.GetCompanyIdByDepartmentIdAsync(employee.DepartmentId, cancellationToken);
          
            return new EmployeeWithDetailsDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Phone = employee.Phone,
                CompanyId = companyId,
                Department = new DepartmentDetailsDto(department.Name, department.Phone),
                Passport = passport != null ? new PassportDetailsDto(passport.Type, passport.Number) : null
            };
        }
    }

}
