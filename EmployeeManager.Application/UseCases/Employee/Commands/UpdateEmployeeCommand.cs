using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Employee.Commands
{
    public record UpdateEmployeeCommand(int Id, string Name, string Surname, string Phone, int DepartmentId, int? PassportId) : IRequest<EmployeeDto>;

    public class UpdateEmployeeHandler(
    ILogger<UpdateEmployeeHandler> logger,
    IValidator<UpdateEmployeeCommand> validator,
    ICompanyRepository departmentRepository,
    IEmployeeRepository employeeRepository
    ) : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating employee with Id {EmployeeId}", command.Id);

            await validator.ValidateAndThrowAsync(command);

            bool departmentExists = await departmentRepository.ExistsAsync(command.DepartmentId);

            if (!departmentExists)
                throw new NotFoundException($"Department with id {command.DepartmentId} does not exist");            

            if (command.PassportId is not null &&
                employeeRepository.GetByPassportIdAsync(command.PassportId.Value, cancellationToken) is not null)
                throw new BusinessException($"Passport with id {command.PassportId.Value} already used");

            EmployeeEntity? employee = await employeeRepository.GetByPhoneAndDepartmentIdAsync(command.Phone, command.DepartmentId, cancellationToken);

            if (employee is not null)
                throw new BusinessException(
                    $"Employee with phone {command.Phone} in department {command.DepartmentId} already exists");

            EmployeeEntity employeeToUpdate = command.ToEntity();
        
            EmployeeEntity? updatedEmployeeEntity = await employeeRepository.UpdateAsync(employeeToUpdate, cancellationToken);

            if (updatedEmployeeEntity is null)            
                throw new NotFoundException($"Employee with id {command.Id} does not exist");           

            logger.LogInformation("Employee with id {EmployeeId} was updated", updatedEmployeeEntity.Id);

            return updatedEmployeeEntity.ToDto();
        }
    }
}