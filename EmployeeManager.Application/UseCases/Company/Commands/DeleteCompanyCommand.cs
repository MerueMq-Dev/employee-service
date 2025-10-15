using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.UseCases.Company.Commands
{
    public record DeleteCompanyCommand(int Id) : IRequest<CompanyDto>;



    public class DeleteCompanyHandler(
        IValidator<DeleteCompanyCommand> validator,
        ICompanyRepository companyRepository
        ) : IRequestHandler<DeleteCompanyCommand, CompanyDto>
    {
        public async Task<CompanyDto> Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
        {

            await validator.ValidateAndThrowAsync(command);

            CompanyEntity? deletedComapnyEntity = await companyRepository.DeleteAsync(command.Id, cancellationToken);

            if (deletedComapnyEntity is null)
            {
                throw new NotFoundException($"Company with id {command.Id} does not exist");
            }

            CompanyDto deletedCompany = new()
            {
                Id = deletedComapnyEntity.Id,
                Address = deletedComapnyEntity.Address,
                Name = deletedComapnyEntity.Name
            };

            return deletedCompany;
        }
    }
}
