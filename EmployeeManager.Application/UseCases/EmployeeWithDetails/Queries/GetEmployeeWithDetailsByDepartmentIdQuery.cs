using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Employee.Queries;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Exceptions;
using EmployeManager.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.UseCases.EmployeeWithDetails.Queries
{
    public record GetEmployeeWithDetailsByDepartmentIdQuery(int Id) : IRequest<IEnumerable<EmployeeWithDetailsDto>>;

    public class GetEmployeeWithDetailsByDepartmentIdHandler(
        IValidator<GetEmployeeWithDetailsByDepartmentIdQuery> validator,
        IEmployeeRepository employeeRepository, 
        ICompanyRepository companyRepository
        ) :
        IRequestHandler<GetEmployeeWithDetailsByDepartmentIdQuery, IEnumerable<EmployeeWithDetailsDto>>
    {
        public async Task<IEnumerable<EmployeeWithDetailsDto>> Handle(GetEmployeeWithDetailsByDepartmentIdQuery query, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(query);

            bool companyExists = await companyRepository.ExistsAsync(query.Id);

            if (!companyExists)
            {
                throw new NotFoundException($"Department with id {query.Id} does not exist");
            }

            return (await employeeRepository.GetEmployeeWithDetailsByDepartmentIdAsync(query.Id, cancellationToken))
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
