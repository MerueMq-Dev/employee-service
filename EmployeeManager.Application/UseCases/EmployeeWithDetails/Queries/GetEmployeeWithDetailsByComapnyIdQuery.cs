using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Employee.Queries;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Exceptions;
using EmployeManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.UseCases.EmployeeWithDetails.Queries
{
    public record GetEmployeeWithDetailsByComapnyIdQuery(int Id) : IRequest<IEnumerable<EmployeeWithDetailsDto>>;

    public class GetEmployeeWithDetailsByComapnyIdHandler(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository) :
        IRequestHandler<GetEmployeeWithDetailsByComapnyIdQuery, IEnumerable<EmployeeWithDetailsDto>>
    {
        public async Task<IEnumerable<EmployeeWithDetailsDto>> Handle(GetEmployeeWithDetailsByComapnyIdQuery query, CancellationToken cancellationToken)
        {

            bool companyExists = await companyRepository.ExistsAsync(query.Id);

            if (!companyExists)
            {
                throw new NotFoundException($"Company with id {query.Id} does not exist");
            }

            return (await employeeRepository.GetEmployeeWithDetailsByCompanyIdAsync(query.Id, cancellationToken))
          .Select(v =>
          {
              return new EmployeeWithDetailsDto
              {
                  Id = v.Id,
                  Name = v.Name,
                  Surname = v.Surname,
                  CompanyId = v.CompanyId,
                  Phone = v.Phone,
                  Department = new DepartmentDetailsDto(
                  v.Department.Name,
                  v.Department.Phone),
                  Passport = new PassportDetailsDto(v.Passport.Type, v.Passport.Number)
              };
          });
        }
    }
}
