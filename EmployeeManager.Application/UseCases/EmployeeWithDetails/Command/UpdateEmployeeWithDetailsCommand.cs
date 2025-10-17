using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace EmployeeManager.Application.UseCases.EmployeeWithDetails.Command
{
    public record UpdateEmployeeWithDetailsCommand : IRequest<EmployeeWithDetailsDto>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public int? CompanyId { get; set; }

        public DepartmentDetailsDto? Department { get; set; }
        public PassportDetailsDto? Passport { get; set; }
    }

    public class UpdateEmployeeWithDetailsHandler(
    IValidator<UpdateEmployeeWithDetailsCommand> validator,
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    IPassportRepository passportRepository,
    ICompanyRepository companyRepository
) : IRequestHandler<UpdateEmployeeWithDetailsCommand, EmployeeWithDetailsDto>
    {
        public async Task<EmployeeWithDetailsDto> Handle(
              UpdateEmployeeWithDetailsCommand command,
              CancellationToken cancellationToken)
        {

            await validator.ValidateAndThrowAsync(command, cancellationToken);

            var existingEmployee = await employeeRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingEmployee == null)
                throw new NotFoundException($"Employee with id {command.Id} not found");

            var currentDepartment = await departmentRepository
                .GetByIdAsync(existingEmployee.DepartmentId, cancellationToken);
            if (currentDepartment == null)
                throw new NotFoundException("Current department not found");

            PassportEntity? currentPassport = null;
            if (existingEmployee.PassportId.HasValue)
                currentPassport = await passportRepository
                    .GetByIdAsync(existingEmployee.PassportId.Value, cancellationToken);


            DepartmentEntity? newDepartment = null;
            if (command.Department != null)
            {
                var departmentResult = await ProcessDepartmentUpdateAsync(command, currentDepartment, cancellationToken);

                newDepartment = departmentResult;
                existingEmployee.DepartmentId = newDepartment.Id;
            }

            if (command.Passport != null)
            {
                var passportResult = await ProcessPassportUpdateAsync(command, existingEmployee, currentPassport, cancellationToken);
                existingEmployee.PassportId = passportResult;
            }

            UpdateEmployeeFields(existingEmployee, command);

            await employeeRepository.UpdateAsync(existingEmployee, cancellationToken);

            var updatedEmployee = await LoadEmployeeWithDetailsAsync(existingEmployee.Id, cancellationToken);
            return updatedEmployee;

        }

        private async Task<DepartmentEntity> ProcessDepartmentUpdateAsync(
            UpdateEmployeeWithDetailsCommand command,
            DepartmentEntity currentDepartment,
            CancellationToken ct)
        {
            var targetCompanyId = command.CompanyId ?? currentDepartment.CompanyId;

            var company = await companyRepository.GetByIdAsync(targetCompanyId, ct);
            if (company == null)
                throw new NotFoundException($"Company with id {targetCompanyId} not found");

            var department = await departmentRepository.GetByNameAndCompanyIdAsync(
                command.Department!.Name, targetCompanyId, ct);

            if (department == null)
                throw new NotFoundException($"Department '{command.Department.Name}' not found in company {targetCompanyId}");

            return department;
        }

        private async Task<int> ProcessPassportUpdateAsync(
            UpdateEmployeeWithDetailsCommand command,
            EmployeeEntity employee,
            PassportEntity? currentPassport,
            CancellationToken ct)
        {

            var existingPassport = await passportRepository.GetByNumberAsync(command.Passport!.Number, ct);

            if (existingPassport != null)
            {
                var employeeWithPassport = await employeeRepository.GetByPassportIdAsync(existingPassport.Id, ct);
                if (employeeWithPassport != null && employeeWithPassport.Id != employee.Id)
                {
                    throw new BusinessException($"Passport with number '{command.Passport.Number}' already assigned to employee {employeeWithPassport.Id}");
                }

                if (existingPassport.Type != command.Passport.Type)
                {
                    existingPassport.Type = command.Passport.Type;
                    await passportRepository.UpdateAsync(existingPassport, ct);
                }

                return existingPassport.Id;
            }

            throw new NotFoundException($"Passport with number '{command.Passport.Number}' was not found");
        }

        private void UpdateEmployeeFields(EmployeeEntity employee, UpdateEmployeeWithDetailsCommand command)
        {
            if (command.Name != null)
                employee.Name = command.Name;

            if (command.Surname != null)
                employee.Surname = command.Surname;

            if (command.Phone != null)
                employee.Phone = command.Phone;
        }

        private async Task<EmployeeWithDetailsDto> LoadEmployeeWithDetailsAsync(int employeeId, CancellationToken ct)
        {
            var employee = await employeeRepository.GetByIdAsync(employeeId, ct);
            if (employee == null)
                throw new InvalidOperationException($"Employee {employeeId} not found after update");

            var department = await departmentRepository.GetByIdAsync(employee.DepartmentId, ct);
            if (department == null)
                throw new InvalidOperationException($"Department {employee.DepartmentId} not found");

            var company = await companyRepository.GetByIdAsync(department.CompanyId, ct);
            if (company == null)
                throw new InvalidOperationException($"Company {department.CompanyId} not found");

            PassportEntity? passport = null;
            if (employee.PassportId.HasValue)
            {
                passport = await passportRepository.GetByIdAsync(employee.PassportId.Value, ct);
            }

            return MapToDto(employee, department, company, passport);
        }

        private EmployeeWithDetailsDto MapToDto(
            EmployeeEntity employee,
            DepartmentEntity department,
            CompanyEntity company,
            PassportEntity? passport)
        {
            return new EmployeeWithDetailsDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Phone = employee.Phone,
                CompanyId = company.Id,
                Department = new DepartmentDetailsDto(department.Name, department.Phone),
                Passport = passport != null ? new PassportDetailsDto(passport.Type, passport.Number) : null
            };
        }
    }

}

