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
    public record UpdateDepartmentCommand(int Id, string Name, string Phone, int CompanyId) : IRequest<DepartmentDto>;

    public class UpdateDepartmentHandler(
    IValidator<UpdateDepartmentCommand> validator,
    ICompanyRepository companyRepository,
    IDepartmentRepository departmentRepository,
    ILogger<UpdateDepartmentHandler> logger
    ) : IRequestHandler<UpdateDepartmentCommand, DepartmentDto>
    {
        public async Task<DepartmentDto> Handle(UpdateDepartmentCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating department with id {DepartmentName}", command.Id);
           
            await validator.ValidateAndThrowAsync(command);

            bool companyExists = await companyRepository.ExistsAsync(command.CompanyId);

            if (!companyExists)           
                throw new NotFoundException($"Company with id {command.CompanyId} does not exist");
            
            var currentDepartment = await departmentRepository.GetByIdAsync(command.Id);

            if (currentDepartment is null)
                throw new NotFoundException($"Department with id {command.CompanyId} does not exist");
            
            bool depatmentWithNameExists = await departmentRepository
                .ExistsByNameAndCompanyIdAsync(command.Name, command.CompanyId);

            if(depatmentWithNameExists)
                throw new BusinessException($"Department with name {command.Name} already exists " +
                $"in Company with {command.CompanyId}");

            bool depatmentWithNumberExists = await departmentRepository
                .ExistsByPhoneAndCompanyIdAsync(command.Phone, command.CompanyId, cancellationToken);

            if (depatmentWithNumberExists && command.Phone != currentDepartment.Phone)
                throw new BusinessException($"Department with phone {command.Phone} already used");           

            DepartmentEntity departmentToUpdate = command.ToEntity();       
            DepartmentEntity? updatedDepartmentEntity = await departmentRepository
                .UpdateAsync(departmentToUpdate, cancellationToken);

            if (updatedDepartmentEntity is null)           
                throw new NotFoundException($"Department with id {command.Id} does not exist");
        
            logger.LogInformation("Department with id {DepartmentName} was updated", updatedDepartmentEntity.Id);
         
            return updatedDepartmentEntity.ToDto();
        }
    }
}