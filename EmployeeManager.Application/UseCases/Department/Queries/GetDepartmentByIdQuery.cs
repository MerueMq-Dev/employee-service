using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Company.Queries
{
    public record GetDepartmentByIdQuery(int Id): IRequest<DepartmentDto>;

    public class GetDepartmentByIdHandler(
        ILogger<GetDepartmentByIdHandler> logger,
        IValidator<GetDepartmentByIdQuery> validator,
        IDepartmentRepository departmentRepository
        ) : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto>
    {
        public async Task<DepartmentDto> Handle(GetDepartmentByIdQuery query, CancellationToken cancellationToken)
        {

            logger.LogInformation("Fetching department with id {id}", query.Id);

            await validator.ValidateAndThrowAsync(query);

            DepartmentEntity? departmentEntity = await departmentRepository.GetByIdAsync(query.Id, cancellationToken);

            if (departmentEntity is null)
            {
                throw new NotFoundException($"Department with id {query.Id} does not exist");
            }

            logger.LogInformation("Department with id {id} was retrieved", query.Id);

            return departmentEntity.ToDto(); ;
        }
    }
}
