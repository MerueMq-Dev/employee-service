using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Employee.Queries;
using EmployeeManager.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.UseCases.EmployeeWithDetails.Queries
{
    public record GetAllEmployeesWithDetailsQuery() : IRequest<IEnumerable<EmployeeWithDetailsDto>>;

    public class GetAllEmployeesWithDetailsHandler(IEmployeeRepository employeeRepository) :
        IRequestHandler<GetAllEmployeesWithDetailsQuery, IEnumerable<EmployeeWithDetailsDto>>
    {
        public async Task<IEnumerable<EmployeeWithDetailsDto>> Handle(GetAllEmployeesWithDetailsQuery request, CancellationToken cancellationToken)
        {
            return (await employeeRepository.GetAllEmployeeWithDetailsAsync(cancellationToken))
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
