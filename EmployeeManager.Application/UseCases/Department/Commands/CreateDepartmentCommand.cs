using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

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
    IDepartmentRepository departmentRepository,
    ILogger<CreateDepartmentHandler> logger
    ) 
    : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken)
    { 

        logger.LogInformation("Creating department {DepartmentName} for company {CompanyId}",
            command.Name, command.CompanyId);
        
        await validator.ValidateAndThrowAsync(command);      

        bool companyExists = await companyRepository.ExistsAsync(command.CompanyId, cancellationToken);

        if (!companyExists)
        {
            throw new NotFoundException($"Company with {command.CompanyId} does not exist");
        }

        bool departmentExists = await departmentRepository.ExistsByNameAndCompanyIdAsync(
                    command.Name, command.CompanyId, cancellationToken);

        if (departmentExists)
            throw new BusinessException($"Department with name {command.Name} already exists " +
                $"in Company with {command.CompanyId}");
        

        departmentExists = await departmentRepository.ExistsByPhoneAndCompanyIdAsync(
                    command.Phone, command.CompanyId, cancellationToken);

        if (departmentExists)
        {
            throw new BusinessException($"Department with phone {command.Phone} already exists " +
                $"in Company with {command.CompanyId}");
        }

        DepartmentEntity departmentToCreate = command.ToEntity();


        DepartmentEntity createdDepartmentEntity = await departmentRepository.CreateAsync(departmentToCreate,
            cancellationToken);

        logger.LogInformation("Department with {DepartmentName} was created for company {CompanyId}",
            createdDepartmentEntity.Name, createdDepartmentEntity.CompanyId);

        DepartmentDto createdDepartment = createdDepartmentEntity.ToDto();

        return createdDepartment;
    }
}