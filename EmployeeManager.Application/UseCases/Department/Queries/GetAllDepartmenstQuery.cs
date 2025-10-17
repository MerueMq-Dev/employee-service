using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Company.Queries
{
    public record GetAllDepartmenstQuery(): IRequest<IEnumerable<DepartmentDto>>;

    public class GetAllDepartmenstHandler(
        ILogger<GetAllDepartmenstHandler> logger,
        IDepartmentRepository departmentRepository
        ) : IRequestHandler<GetAllDepartmenstQuery, IEnumerable<DepartmentDto>>
    {
        public async Task<IEnumerable<DepartmentDto>> Handle(GetAllDepartmenstQuery query, 
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching all departments");
            IEnumerable<DepartmentDto> allDepartments = (await departmentRepository.GetAllAsync(cancellationToken))
                .Select(c => c.ToDto());

            logger.LogInformation("All departments were retrieved");
            return allDepartments;
        }
    }
}
