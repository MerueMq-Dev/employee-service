using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.UseCases.Company.Queries
{
    public record GetAllCompaniesQuery(): IRequest<IEnumerable<CompanyDto>>;

    public class GetAllCompaniesHandler(ICompanyRepository companyRepository) : IRequestHandler<GetAllCompaniesQuery, IEnumerable<CompanyDto>>
    {
        public async Task<IEnumerable<CompanyDto>> Handle(GetAllCompaniesQuery query, CancellationToken cancellationToken)
        {
            return (await companyRepository.GetAllAsync())
                .Select(c => new CompanyDto { Id = c.Id, Address = c.Address, Name = c.Name });
        }
    }
}
