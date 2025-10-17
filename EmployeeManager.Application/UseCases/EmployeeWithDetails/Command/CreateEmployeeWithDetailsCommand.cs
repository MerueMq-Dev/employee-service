using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Validators.EmployeeWithDetails;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using EmployeManager.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManager.Application.UseCases.EmployeeWithDetails.Commands;

public record CreateEmployeeWithDetailsCommand : IRequest<EmployeeWithDetailsDto>
{
    public string Name { get; set; }

    public string Surname { get; set; }

    public string Phone { get; set; }
 
    public int CompanyId { get; set; }

    public DepartmentDetailsDto Department { get; set; }

    public PassportDetailsDto Passport { get; set; }

}


public class CreateEmployeeWithDetailsHandler(
    IValidator<CreateEmployeeWithDetailsCommand> validator,
    ICompanyRepository companyRepository,
    IPassportRepository passportRepository,
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository
    ) 
    : IRequestHandler<CreateEmployeeWithDetailsCommand, EmployeeWithDetailsDto>
{
    public async Task<EmployeeWithDetailsDto> Handle(
           CreateEmployeeWithDetailsCommand command,
           CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command);

        var company = await companyRepository.GetByIdAsync(command.CompanyId, cancellationToken);
        if (company is null)
            throw new NotFoundException($"Company with id {command.CompanyId} does not exist");

        var department = await departmentRepository.GetByNameAndCompanyIdAsync(
            command.Department.Name, command.CompanyId, cancellationToken);
        if (department is null)
            throw new NotFoundException($"Department '{command.Department.Name}' does not exist in company {command.CompanyId}");
        
        var passport = await passportRepository.GetByNumberAsync(command.Passport.Number, cancellationToken);
        if (passport is null)
            throw new NotFoundException($"Passport with number '{command.Passport.Number}' does not exist");

        var employeeWithPassport = await employeeRepository.GetByPassportIdAsync(passport.Id, cancellationToken);
        if (employeeWithPassport != null)
            throw new BusinessException($"Passport is already assigned to employee {employeeWithPassport.Id}");

        
        var employeeWithSamePhone = await employeeRepository.GetByPhoneAndDepartmentIdAsync(command.Phone, department.Id, cancellationToken);
        if (employeeWithSamePhone != null)
            throw new BusinessException($"Employee with phone '{command.Phone}' already exists");

        var employeeToCreate = new EmployeeEntity()
        {
            Name = command.Name,
            Surname = command.Surname,
            Phone = command.Phone,
            DepartmentId = department.Id,
            PassportId = passport.Id
        };

        var createdEmployeeEntity = await employeeRepository.CreateAsync(employeeToCreate, cancellationToken);

        return new EmployeeWithDetailsDto
        {
            Id = createdEmployeeEntity.Id,
            Name = createdEmployeeEntity.Name,
            Surname = createdEmployeeEntity.Surname,
            Phone = createdEmployeeEntity.Phone,
            CompanyId = company.Id,
            Department = new DepartmentDetailsDto(department.Name, department.Phone),
            Passport = new PassportDetailsDto(passport.Type, passport.Number)
        };
    }
}