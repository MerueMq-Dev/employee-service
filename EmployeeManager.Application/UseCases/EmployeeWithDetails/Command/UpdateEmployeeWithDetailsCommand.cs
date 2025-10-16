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
) : IRequestHandler<UpdateEmployeeWithDetailsCommand, EmployeeWithDetailsDto>
    {
        public async Task<EmployeeWithDetailsDto> Handle(
            UpdateEmployeeWithDetailsCommand command,
            CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command, cancellationToken);

            EmployeeEntity? employee = await employeeRepository.GetByIdAsync(command.Id, cancellationToken);
            if (employee is null)
                throw new NotFoundException($"Employee with id {command.Id} does not exist");

            UpdatePersonalData(employee, command);

            await UpdatePhoneIfNeededAsync(employee, command, cancellationToken);

            var department = await UpdateDepartmentIfNeededAsync(employee, command, cancellationToken);

            var passport = await UpdatePassportIfNeededAsync(employee, command, cancellationToken);

            await employeeRepository.UpdateAsync(employee, cancellationToken);
            
            return new EmployeeWithDetailsDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Phone = employee.Phone,
                CompanyId = department.CompanyId,
                Department = new DepartmentDetailsDto(department.Name, department.Phone),
                Passport = passport != null
                    ? new PassportDetailsDto(passport.Type, passport.Number)
                    : null
            };
        }

        private static void UpdatePersonalData(EmployeeEntity employee, UpdateEmployeeWithDetailsCommand command)
        {
            if (!string.IsNullOrWhiteSpace(command.Name))
                employee.Name = command.Name;

            if (!string.IsNullOrWhiteSpace(command.Surname))
                employee.Surname = command.Surname;
        }

        private async Task UpdatePhoneIfNeededAsync(
            EmployeeEntity employee,
            UpdateEmployeeWithDetailsCommand command,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Phone))
                return;

            var employeeWithPhone = await employeeRepository.GetByPhoneAsync(command.Phone, cancellationToken);

            if (employeeWithPhone is not null && employeeWithPhone.Id != command.Id)
                throw new BusinessException($"Phone '{command.Phone}' is already in use by employee {employeeWithPhone.Id}");

            employee.Phone = command.Phone;
        }

        private async Task<DepartmentEntity> UpdateDepartmentIfNeededAsync(
            EmployeeEntity employee,
            UpdateEmployeeWithDetailsCommand command,
            CancellationToken cancellationToken)
        {
            if (command.Department is null)
                return await departmentRepository.GetByIdAsync(employee.DepartmentId, cancellationToken);

            var newDepartment = await departmentRepository.GetByNameAsync(command.Department.Name, cancellationToken);
            if (newDepartment is null)
                throw new NotFoundException($"Department with name '{command.Department.Name}' does not exist");

            var currentDepartment = await departmentRepository.GetByIdAsync(employee.DepartmentId, cancellationToken);

            if (newDepartment.CompanyId != currentDepartment.CompanyId)
            {
                throw new BusinessException(
                    $"Cannot transfer employee to department '{command.Department.Name}' " +
                    $"because it belongs to company {newDepartment.CompanyId}, " +
                    $"but employee currently belongs to company {currentDepartment.CompanyId}");
            }

            employee.DepartmentId = newDepartment.Id;
            return newDepartment;
        }

        private async Task<PassportEntity?> UpdatePassportIfNeededAsync(
            EmployeeEntity employee,
            UpdateEmployeeWithDetailsCommand command,
            CancellationToken cancellationToken)
        {
            if (command.Passport is null)
            {
                return employee.PassportId.HasValue
                    ? await passportRepository.GetByIdAsync(employee.PassportId.Value, cancellationToken)
                    : null;
            }

            var passport = await passportRepository.GetByNumberAsync(command.Passport.Number, cancellationToken);
            if (passport is null)
                throw new NotFoundException($"Passport with number '{command.Passport.Number}' does not exist");

            var employeeWithPassport = await employeeRepository.GetByPassportIdAsync(passport.Id, cancellationToken);
            if (employeeWithPassport is not null && employeeWithPassport.Id != command.Id)
            {
                throw new BusinessException(
                    $"Passport with number '{command.Passport.Number}' is already used by employee {employeeWithPassport.Id}");
            }

            employee.PassportId = passport.Id;
            return passport;
        }
    }
}
