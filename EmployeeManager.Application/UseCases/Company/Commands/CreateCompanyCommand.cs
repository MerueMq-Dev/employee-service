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
    public record CreateCompanyCommand : IRequest<CompanyDto>
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
    }

    public class CreateCompanyHandler(
        ILogger<CreateCompanyHandler> logger,
       IValidator<CreateCompanyCommand> validator,
       ICompanyRepository companyRepository
       ) : IRequestHandler<CreateCompanyCommand, CompanyDto>
    {
        public async Task<CompanyDto> Handle(
            CreateCompanyCommand command, 
            CancellationToken cancellationToken)
        {

            logger.LogInformation("Creating company with name {CompanyName}", command.Name);

            await validator.ValidateAndThrowAsync(command);

            CompanyEntity companyToCreate = command.ToEntity();

            bool companyExists = await companyRepository.ExistsByNameAsync(command.Name);

            if (companyExists)
                throw new BusinessException($"Company with name {command.Name} already exists");

            CompanyEntity createdCompanyEntity = await companyRepository.CreateAsync(companyToCreate, 
                cancellationToken);           

            logger.LogInformation("Company with name {ComapanyName} was created", command.Name);

            return createdCompanyEntity.ToDto();
        }
    }
}
