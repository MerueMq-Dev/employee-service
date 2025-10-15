using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.Company.Queries
{
    public record GetDepartmentByIdQuery(int Id): IRequest<DepartmentDto>;

    public class GetDepartmentByIdHandler(
        IValidator<GetDepartmentByIdQuery> validator,
        IDepartmentRepository departmentRepository
        ) : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
    {
        public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery query, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(query);

            DepartmentEntity? departmentEntity = await departmentRepository.GetByIdAsync(query.Id, cancellationToken);

            if (departmentEntity is null)
            {
                throw new NotFoundException($"Department with id {query.Id} does not exist");
            }

            DepartmentDto foundDepartment = new()
            {
                Id = departmentEntity.Id,
                Name = departmentEntity.Name,
                Phone = departmentEntity.Phone,
                CompanyId = departmentEntity.CompanyId,
            };

            return foundDepartment;
        }
    }
}
