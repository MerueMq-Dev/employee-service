using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.Department.Commands
{
    public record DeleteDepartmentCommand(int Id) : IRequest<DepartmentDto>;

    public class DeleteDepartmentHandler(
     IValidator<DeleteDepartmentCommand> validator,
     IDepartmentRepository departmentRepository
     ) : IRequestHandler<DeleteDepartmentCommand, DepartmentDto>
    {
        public async Task<DepartmentDto> Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command);

            DepartmentEntity? deletedDepartmentEntity = await departmentRepository
                .DeleteAsync(command.Id, cancellationToken);

            if (deletedDepartmentEntity is null)
            {
                throw new NotFoundException($"Department with id {command.Id} does not exist");
            }

            DepartmentDto deletedDepartment = new()
            {
                Id = deletedDepartmentEntity.Id,
                Name = deletedDepartmentEntity.Name,
                Phone = deletedDepartmentEntity.Phone,
                CompanyId = deletedDepartmentEntity.CompanyId
            };

            return deletedDepartment;
        }
    }
}
