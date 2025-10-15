using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.UseCases.Company.Commands
{
    public record CreateCompanyCommand : IRequest<CompanyDto>
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
    }

    public class CreateCompanyHandler(
       IValidator<CreateCompanyCommand> validator,
       ICompanyRepository companyRepository
       ) : IRequestHandler<CreateCompanyCommand, CompanyDto>
    {
        public async Task<CompanyDto> Handle(
            CreateCompanyCommand command, 
            CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command);

            CompanyEntity companyToCreate = new CompanyEntity()
            {
                Address = command.Address,
                Name = command.Name
            };

            CompanyEntity createdCompanyEntity = await companyRepository.CreateAsync(companyToCreate, 
                cancellationToken);

            CompanyDto createdCompany = new CompanyDto
            {
                Address = command.Address,
                Name = command.Name,
                Id = createdCompanyEntity.Id
            };

            return createdCompany;
        }
    }
}
