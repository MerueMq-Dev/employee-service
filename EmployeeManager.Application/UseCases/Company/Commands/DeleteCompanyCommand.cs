using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.UseCases.Company.Commands
{
    public record DeleteCompanyCommand(int Id) : IRequest<CompanyDto>;

    public class DeleteCompanyHandler(
        ILogger<DelegatingHandler> logger,
        IValidator<DeleteCompanyCommand> validator,
        ICompanyRepository companyRepository
        ) : IRequestHandler<DeleteCompanyCommand, CompanyDto>
    {
        public async Task<CompanyDto> Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting company with id {CompanyId}", command.Id);           

            await validator.ValidateAndThrowAsync(command);            

            CompanyEntity? deletedComapnyEntity = await companyRepository
                .DeleteAsync(command.Id, cancellationToken);

            if (deletedComapnyEntity is null)
            {
                throw new NotFoundException($"Company with id {command.Id} does not exist");
            }

            logger.LogInformation("Company with id {ComapnyId} was deleted", command.Id);

            return deletedComapnyEntity.ToDto();
        }
    }
}
