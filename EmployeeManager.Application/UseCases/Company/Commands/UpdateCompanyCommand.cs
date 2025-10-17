using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.UseCases.Company.Commands
{
    public record UpdateCompanyCommand(int Id, string Name, string Address) : IRequest<CompanyDto>;

    public class UpdateCompanyHandler(
    ILogger<UpdateCompanyHandler> logger,
    IValidator<UpdateCompanyCommand> validator,
    ICompanyRepository companyRepository
    ) : IRequestHandler<UpdateCompanyCommand, CompanyDto>
    {
        public async Task<CompanyDto> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
        {            
            logger.LogInformation("Updating company with id {CompanyId}", command.Id);

            await validator.ValidateAndThrowAsync(command);
            
            bool companyExists = await companyRepository.ExistsByNameAsync(command.Name);
            
            if (companyExists)
                throw new BusinessException($"Company with name {command.Name} already exists");

            var companyUpdateEntity = command.ToEntity();
            
            CompanyEntity? updatedComapnyEntity = await companyRepository.UpdateAsync(companyUpdateEntity, cancellationToken);

            if (updatedComapnyEntity is null)
            {
                throw new NotFoundException($"Company with id {command.Id} does not exist");
            }
                     
            return updatedComapnyEntity.ToDto();
        }
    }
}