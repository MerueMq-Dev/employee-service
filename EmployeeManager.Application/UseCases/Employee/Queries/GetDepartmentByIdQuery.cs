using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.Employee.Queries
{
    public record GetEmployeeByIdQuery(int Id): IRequest<EmployeeDto>;

    public class GetEmployeeByIdHandler(
        IValidator<GetEmployeeByIdQuery> validator,
        IEmployeeRepository employeeRepository
        ) : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {

            await validator.ValidateAndThrowAsync(query);

            EmployeeEntity? employeeEntity = await employeeRepository.GetByIdAsync(query.Id, cancellationToken);

            if (employeeEntity is null)
            {
                throw new NotFoundException($"Employee with id {query.Id} does not exist");
            }

            EmployeeDto foundEmployee = new()
            {
                Id = employeeEntity.Id,
                Name = employeeEntity.Name,
                Surname = employeeEntity.Surname,
                Phone = employeeEntity.Phone,
                DepartmentId = employeeEntity.DepartmentId,
            };

            return foundEmployee;
        }
    }
}
