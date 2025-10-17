using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Employee.Queries
{
    public record GetEmployeeByIdQuery(int Id): IRequest<EmployeeDto>;

    public class GetEmployeeByIdHandler(
        ILogger<GetEmployeeByIdHandler> logger,
        IValidator<GetEmployeeByIdQuery> validator,
        IEmployeeRepository employeeRepository
        ) : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {

            logger.LogInformation("Fetching employee with id {EmployeeId}", query.Id);

            await validator.ValidateAndThrowAsync(query);

            EmployeeEntity? employeeEntity = await employeeRepository.GetByIdAsync(query.Id, cancellationToken);

            if (employeeEntity is null)
            {
                throw new NotFoundException($"Employee with id {query.Id} does not exist");
            }

            logger.LogInformation("Employee with id {EmployeeId} was retrieved", query.Id);

            return employeeEntity.ToDto();
        }
    }
}
