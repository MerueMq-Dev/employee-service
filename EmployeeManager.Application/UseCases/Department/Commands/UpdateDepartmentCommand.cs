using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.Department.Commands
{
    public record UpdateDepartmentCommand(int Id, string Name, string Phone, int CompanyId) : IRequest<DepartmentDto>;

    public class UpdateDepartmentHandler(
    IValidator<UpdateDepartmentCommand> validator,
    ICompanyRepository companyRepository,
    IDepartmentRepository departmentEntity
    ) : IRequestHandler<UpdateDepartmentCommand, DepartmentDto>
    {
        public async Task<DepartmentDto> Handle(UpdateDepartmentCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command);


            bool companyExists = await companyRepository.ExistsAsync(command.CompanyId);

            if (!companyExists)
            {
                throw new NotFoundException($"Company with id {command.CompanyId} does not exist");
            }


            DepartmentEntity departmentToUpdate = new()
            {
                Id = command.Id,
                Name = command.Name,
                Phone = command.Phone,
                CompanyId = command.CompanyId
            };
        
            DepartmentEntity? updatedDepartmentEntity = await departmentEntity.UpdateAsync(departmentToUpdate, cancellationToken);

            if (updatedDepartmentEntity is null)
            {
                throw new NotFoundException($"Department with id {command.Id} does not exist");
            }

            DepartmentDto updatedDepartment = new DepartmentDto
            {
                Id = updatedDepartmentEntity.Id,
                Name = updatedDepartmentEntity.Name,
                Phone = updatedDepartmentEntity.Phone,
                CompanyId = updatedDepartmentEntity.CompanyId
            };

            return updatedDepartment;
        }
    }
}