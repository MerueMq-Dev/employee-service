using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using MediatR;

namespace EmployeeManager.Application.Company.Queries
{
    public record GetAllDepartmenstQuery(): IRequest<IEnumerable<DepartmentDto>>;

    public class GetAllDepartmenstHandler(IDepartmentRepository departmentRepository) : IRequestHandler<GetAllDepartmenstQuery, IEnumerable<DepartmentDto>>
    {
        public async Task<IEnumerable<DepartmentDto>> Handle(GetAllDepartmenstQuery query, CancellationToken cancellationToken)
        {
            return (await departmentRepository.GetAllAsync())
                .Select(c => new DepartmentDto { Id = c.Id, Name = c.Name, Phone = c.Phone, CompanyId = c.CompanyId });
        }
    }
}
