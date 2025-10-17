using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.UseCases.Company.Queries
{
    public record GetAllCompaniesQuery(): IRequest<IEnumerable<CompanyDto>>;

    public class GetAllCompaniesHandler(
        ICompanyRepository companyRepository,
        ILogger<GetAllCompaniesHandler> logger
        ) : IRequestHandler<GetAllCompaniesQuery, IEnumerable<CompanyDto>>
    {
        public async Task<IEnumerable<CompanyDto>> Handle(GetAllCompaniesQuery query, 
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching all companies");
            
            IEnumerable<CompanyDto> companies = (await companyRepository.GetAllAsync(cancellationToken))
                .Select(c => c.ToDto());
            
            logger.LogInformation("All companies were retrieved");
            
            return companies;
        }
    }
}
