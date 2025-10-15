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
    public record GetEmployeeWithDetailsByIdQuery(int Id): IRequest<EmployeeWithDetailsDto>;

    public class GetEmployeeWithDetailsByIdHandler(
        IValidator<GetEmployeeWithDetailsByIdQuery> validator,
        IEmployeeRepository employeeRepository
        ) :
        IRequestHandler<GetEmployeeWithDetailsByIdQuery, EmployeeWithDetailsDto>
    {
        public async Task<EmployeeWithDetailsDto> Handle(GetEmployeeWithDetailsByIdQuery request, CancellationToken cancellationToken)
        {

            await validator.ValidateAndThrowAsync(request);

            EmployeeWithDetailsEntity employeeWithDetailEnity = await employeeRepository.GetEmployeeWithDetailsByIdAsync(request.Id, cancellationToken);

            if (employeeWithDetailEnity is null) {

                throw new NotFoundException($"Empoyee with Id {request.Id} does not exist");
            }

            return new EmployeeWithDetailsDto
            {
                Id = employeeWithDetailEnity.Id,
                Name = employeeWithDetailEnity.Name,
                Surname = employeeWithDetailEnity.Surname,
                CompanyId = employeeWithDetailEnity.CompanyId,
                Phone = employeeWithDetailEnity.Phone,
                Department = new DepartmentDetailsDto(
                       employeeWithDetailEnity.Department.Name,
                       employeeWithDetailEnity.Department.Phone),
                Passport = new PassportDetailsDto(employeeWithDetailEnity.Passport.Type, employeeWithDetailEnity.Passport.Number)
            };
        }
    }
}
