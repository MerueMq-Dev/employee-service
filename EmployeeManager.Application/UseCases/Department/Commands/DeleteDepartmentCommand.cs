using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Department.Commands
{
    public record DeleteDepartmentCommand(int Id) : IRequest<DepartmentDto>;

    public class DeleteDepartmentHandler(
     IValidator<DeleteDepartmentCommand> validator,
     ILogger<DeleteDepartmentHandler> logger,
     IDepartmentRepository departmentRepository
     ) : IRequestHandler<DeleteDepartmentCommand, DepartmentDto>
    {
        public async Task<DepartmentDto> Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting department with id {DepartmentId}", command.Id);
            await validator.ValidateAndThrowAsync(command);

            DepartmentEntity? deletedDepartmentEntity = await departmentRepository
                .DeleteAsync(command.Id, cancellationToken);

            if (deletedDepartmentEntity is null)
            {
                throw new NotFoundException($"Department with id {command.Id} does not exist");
            }

            logger.LogInformation("Department with id {DepartmentId} was deleted", command.Id);

            return deletedDepartmentEntity.ToDto();
        }
    }
}
