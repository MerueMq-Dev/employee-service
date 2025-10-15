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

        CompanyEntity? company = await companyRepository.GetByIdAsync(command.CompanyId, cancellationToken);

        if(company is null)
            throw new NotFoundException($"Company with id {command.CompanyId} does not exist");

        DepartmentEntity? department = await departmentRepository.GetByNameAsync(command.Department.Name, cancellationToken);
        if (department is null)      
            throw new NotFoundException($"Department with name {command.Department.Name} does not exist");

        PassportEntity? passport = await passportRepository.GetByNumberAsync(command.Passport.Number);
        
        if (passport is null)
        {
            throw new BusinessException($"Passport with number {command.Passport.Number} alredy exists");
        }

        EmployeeEntity employeeToCreate = new EmployeeEntity()
        {
            Name = command.Name,
            Surname = command.Surname,
            Phone = command.Phone,
            DepartmentId = department.Id,
            PassportId = passport.Id
        }; 

        EmployeeEntity createdEmployeeEntity = await employeeRepository.CreateAsync(employeeToCreate,
            cancellationToken);

        EmployeeWithDetailsDto createdEmployee = new EmployeeWithDetailsDto
        {
            Id = createdEmployeeEntity.Id,
            Name = createdEmployeeEntity.Name,
            Surname = createdEmployeeEntity.Surname,
            Phone = createdEmployeeEntity.Phone,
            CompanyId = company.Id,
            Department = new DepartmentDetailsDto(department.Name, department.Phone),
            Passport = new PassportDetailsDto(passport.Type, passport.Number)
        };

        return createdEmployee;
    }
}