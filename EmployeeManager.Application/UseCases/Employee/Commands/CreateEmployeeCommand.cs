using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using EmployeManager.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

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
        await validator.ValidateAndThrowAsync(command);

        bool departmentExists = await departmentRepository.ExistsAsync(command.DepartmentId, cancellationToken);

        if (!departmentExists)
        {
            throw new NotFoundException($"Department with {command.DepartmentId} does not exist");
        }

        EmployeeEntity employeeToCreate = new()
        {
            Name = command.Name,
            Surname = command.Surname,
            Phone = command.Phone,
            DepartmentId = command.DepartmentId,
            PassportId = command.PassportId
        };

        EmployeeEntity createdEmployeeEntity = await employeeRepository.CreateAsync(employeeToCreate,
            cancellationToken);

        EmployeeDto createdEmployee = new EmployeeDto
        {
            Id = createdEmployeeEntity.Id,
            Name = createdEmployeeEntity.Name,
            Surname = createdEmployeeEntity.Surname,
            Phone = createdEmployeeEntity.Phone,
            DepartmentId = createdEmployeeEntity.DepartmentId,
            PassportId = createdEmployeeEntity.PassportId
        };

        return createdEmployee;
    }
}