using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.UseCases.Department.Commands;

public record CreateDepartmentCommand : IRequest<DepartmentDto>
{
    public string Name { get; set; }

    public string Phone { get; set; }

    public int CompanyId { get; set; }
}


public class CreateDepartmentHandler(
    IValidator<CreateDepartmentCommand> validator,
    ICompanyRepository companyRepository,
    IDepartmentRepository departmentRepository
    ) 
    : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken)
    { 
        await validator.ValidateAndThrowAsync(command);      

        bool companyExists = await companyRepository.ExistsAsync(command.CompanyId, cancellationToken);

        if (!companyExists)
        {
            throw new NotFoundException($"Company with {command.CompanyId} does not exist");
        }
       

        DepartmentEntity departmentToCreate = new()
        {
            Name = command.Name,
            Phone = command.Phone,
            CompanyId = command.CompanyId
        };

        DepartmentEntity createdDepartmentEntity = await departmentRepository.CreateAsync(departmentToCreate,
            cancellationToken);

        DepartmentDto createdDepartment = new DepartmentDto
        {
            Id = createdDepartmentEntity.Id,
            Name = createdDepartmentEntity.Name,
            Phone = createdDepartmentEntity.Phone,
            CompanyId = createdDepartmentEntity.CompanyId
        };

        return createdDepartment;
    }
}