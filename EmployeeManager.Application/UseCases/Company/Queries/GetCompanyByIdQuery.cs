using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.UseCases.Company.Queries
{
    public record GetCompanyByIdQuery(int Id): IRequest<CompanyDto>;


    public class GetCompanyByIdHandler(
        IValidator<GetCompanyByIdQuery> validator,
        ICompanyRepository companyRepository
        ) : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
    {
        public async Task<CompanyDto> Handle(GetCompanyByIdQuery query, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(query);

            CompanyEntity? companyEntity = await companyRepository.GetByIdAsync(query.Id, cancellationToken);

            if (companyEntity is null)
            {
                throw new NotFoundException($"Company with id {query.Id} does not exist");
            }

            CompanyDto foundCompany = new()
            {
                Id = companyEntity.Id,
                Name = companyEntity.Name,
                Address = companyEntity.Address
            };

            return foundCompany;
        }
    }

}
