using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.UseCases.Company.Commands
{
    public record UpdateCompanyCommand(int Id, string Name, string Address) : IRequest<CompanyDto>;

    public class UpdateCompanyHandler(IValidator<UpdateCompanyCommand> validator,
    ICompanyRepository companyRepository
    ) : IRequestHandler<UpdateCompanyCommand, CompanyDto>
    {
        public async Task<CompanyDto> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command);

            var companyUpdateEntity = new CompanyEntity() { Id = command.Id, Name = command.Name, Address = command.Address };
            CompanyEntity? updatedComapnyEntity = await companyRepository.UpdateAsync(companyUpdateEntity, cancellationToken);

            if (updatedComapnyEntity is null)
            {
                throw new NotFoundException($"Company with id {command.Id} does not exist");
            }

            CompanyDto updatedCompany = new()
            {
                Id = updatedComapnyEntity.Id,
                Address = updatedComapnyEntity.Address,
                Name = updatedComapnyEntity.Name
            };

            return updatedCompany;
        }
    }
}