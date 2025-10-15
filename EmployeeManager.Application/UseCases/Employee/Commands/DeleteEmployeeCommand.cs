using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.Employee.Commands
{
    public record DeleteEmployeeCommand(int Id) : IRequest<EmployeeDto>;

    public class DeleteEmployeeHandler(
     IValidator<DeleteEmployeeCommand> validator,
     IEmployeeRepository employeeRepository
     ) : IRequestHandler<DeleteEmployeeCommand, EmployeeDto>
    {
        public async Task<EmployeeDto> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command);

            EmployeeEntity? deletedEmployeeEntity = await employeeRepository
                .DeleteAsync(command.Id, cancellationToken);

            if (deletedEmployeeEntity is null)
            {
                throw new NotFoundException($"Employee with id {command.Id} does not exist");
            }

            EmployeeDto deletedEmployee = new()
            {
                Id = deletedEmployeeEntity.Id,
                Name = deletedEmployeeEntity.Name,
                Surname = deletedEmployeeEntity.Surname,
                Phone = deletedEmployeeEntity.Phone,
                DepartmentId = deletedEmployeeEntity.DepartmentId,
                PassportId = deletedEmployeeEntity.PassportId
            };

            return deletedEmployee;
        }
    }
}
