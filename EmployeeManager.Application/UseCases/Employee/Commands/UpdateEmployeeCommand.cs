using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.Employee.Commands
{
    public record UpdateEmployeeCommand(int Id, string Name, string Surname, string Phone, int DeportamentId, int? PassportId) : IRequest<EmployeeDto>;

    public class UpdateEmployeeHandler(IValidator<UpdateEmployeeCommand> validator,
    ICompanyRepository departmentRepository,
    IEmployeeRepository employeeRepository
    ) : IRequestHandler<UpdateEmployeeCommand, EmployeeDto>
    {
        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command);

            bool departmentExists = await departmentRepository.ExistsAsync(command.DeportamentId);

            if (!departmentExists)
            {
                throw new NotFoundException($"Department with id {command.DeportamentId} does not exist");
            }


            EmployeeEntity employeeToUpdate = new()
            {
                Name = command.Name,
                Surname = command.Surname,
                Phone = command.Phone,
                DepartmentId = command.DeportamentId,
                PassportId = command.PassportId
            };
        
            EmployeeEntity? updatedEmployeeEntity = await employeeRepository.UpdateAsync(employeeToUpdate, cancellationToken);

            if (updatedEmployeeEntity is null)
            {
                throw new NotFoundException($"Employee with id {command.Id} does not exist");
            }

            EmployeeDto updatedEmployee = new EmployeeDto
            {
                Id = updatedEmployeeEntity.Id,
                Name = updatedEmployeeEntity.Name,
                Phone = updatedEmployeeEntity.Phone,
                DepartmentId = updatedEmployeeEntity.DepartmentId,
                PassportId = updatedEmployeeEntity.PassportId
            };

            return updatedEmployee;
        }
    }
}