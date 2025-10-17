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
    public record DeleteEmployeeCommand(int Id) : IRequest<EmployeeDto>;

    public class DeleteEmployeeHandler(
     ILogger<DeleteEmployeeHandler> logger,    
     IValidator<DeleteEmployeeCommand> validator,
     IEmployeeRepository employeeRepository
     ) : IRequestHandler<DeleteEmployeeCommand, EmployeeDto>
    {
        public async Task<EmployeeDto> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting employee with id {EmployeeId}", command.Id);

            await validator.ValidateAndThrowAsync(command);

            EmployeeEntity? deletedEmployeeEntity = await employeeRepository
                .DeleteAsync(command.Id, cancellationToken);

            if (deletedEmployeeEntity is null)          
                throw new NotFoundException($"Employee with id {command.Id} does not exist");

            logger.LogInformation("Employee with id {EmployeeId} was deleted", command.Id);

            return deletedEmployeeEntity.ToDto();
        }
    }
}
