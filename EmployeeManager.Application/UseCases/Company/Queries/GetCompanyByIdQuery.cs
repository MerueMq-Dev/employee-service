using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.UseCases.Company.Queries
{
    public record GetCompanyByIdQuery(int Id): IRequest<CompanyDto>;

    public class GetCompanyByIdHandler(
        ILogger<GetAllCompaniesHandler> logger,
        IValidator<GetCompanyByIdQuery> validator,
        ICompanyRepository companyRepository
        ) : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
    {
        public async Task<CompanyDto> Handle(GetCompanyByIdQuery query, CancellationToken cancellationToken)
        {

            logger.LogInformation("Fetching company with id {id}", query.Id);
            
            await validator.ValidateAndThrowAsync(query);

            CompanyEntity? companyEntity = await companyRepository.GetByIdAsync(query.Id, cancellationToken);

            if (companyEntity is null)
            {
                throw new NotFoundException($"Company with id {query.Id} does not exist");
            }

            logger.LogInformation("Company with id {id} was retrieved", query.Id);

            return companyEntity.ToDto();
        }
    }
}
