using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Employee.Queries
{
    public record GetAllEmployeesQuery() : IRequest<IEnumerable<EmployeeDto>>;

    public class GetAllEmployeesHandler(
        ILogger<GetAllEmployeesHandler> logger,
        IEmployeeRepository employeeRepository
        ) : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        public async Task<IEnumerable<EmployeeDto>> Handle(
            GetAllEmployeesQuery query, 
            CancellationToken cancellationToken
        )
        {
            logger.LogInformation("Fetching all employees");

            IEnumerable<EmployeeDto> employees = (await employeeRepository.GetAllAsync())
                .Select(e => e.ToDto());

            logger.LogInformation("All companies were retrieved");
            
            return employees;
        }
    }
}
