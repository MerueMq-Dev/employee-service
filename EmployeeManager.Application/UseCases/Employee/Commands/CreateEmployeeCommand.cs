using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using EmployeManager.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.UseCases.Employee.Commands;

public record CreateEmployeeCommand : IRequest<EmployeeDto>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public int DepartmentId { get; set; }
    public  int? PassportId { get; set; }
}

public class CreateEmployeeHandler(
    ILogger<CreateEmployeeHandler> logger,
    IValidator<CreateEmployeeCommand> validator,
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository
    ) 
    : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating employee in Department {DepartmentId} with name {EmployeeName} {EmployeeSurname}",
            command.DepartmentId, command.Name, command.Surname);

        await validator.ValidateAndThrowAsync(command);

        bool departmentExists = await departmentRepository.ExistsAsync(command.DepartmentId, cancellationToken);

        if (!departmentExists)     
            throw new NotFoundException($"Department with {command.DepartmentId} does not exist");
;

        if (command.PassportId is not null &&
            (await employeeRepository.GetByPassportIdAsync(command.PassportId.Value, cancellationToken))  is not null)       
            throw new BusinessException($"Passport with id {command.PassportId.Value} alredy used");
        
        EmployeeEntity? employee = await employeeRepository.GetByPhoneAndDepartmentIdAsync(command.Phone, command.DepartmentId ,cancellationToken);

        if (employee is not null)
            throw new BusinessException(
                $"Employee with phone {command.Phone} in department {command.DepartmentId} already exists");

        EmployeeEntity employeeToCreate = command.ToEntity();

        EmployeeEntity createdEmployeeEntity = await employeeRepository.CreateAsync(employeeToCreate,
            cancellationToken);

        logger.LogInformation("Employee with name {EmployeeName} {EmployeeSurname} was created in Department {DepartmentId} ",
             command.Name, command.Surname, command.DepartmentId);

        return createdEmployeeEntity.ToDto();
    }
}