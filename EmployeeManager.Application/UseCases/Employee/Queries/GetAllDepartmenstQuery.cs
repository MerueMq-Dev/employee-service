using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using MediatR;

namespace EmployeeManager.Application.Employee.Queries
{
    public record GetAllEmployeesQuery() : IRequest<IEnumerable<EmployeeDto>>;

    public class GetAllEmployeesHandler(IEmployeeRepository employeeRepository) : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery query, CancellationToken cancellationToken)
        {
            return (await employeeRepository.GetAllAsync())
                .Select(c => new EmployeeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Surname = c.Surname,
                    Phone = c.Phone,
                    DepartmentId = c.DepartmentId,
                    PassportId = c.PassportId
                });
        }
    }
}
