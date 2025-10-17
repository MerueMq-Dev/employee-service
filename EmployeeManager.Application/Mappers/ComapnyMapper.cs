using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.UseCases.Company.Commands;
using EmployeeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Mappers
{
    public static class ComapnyMapper
    {
        public static CompanyEntity ToEntity(this CreateCompanyCommand command)
        {
            return new CompanyEntity()
            {
                Address = command.Address,
                Name = command.Name
            };
        }

        public static CompanyEntity ToEntity(this UpdateCompanyCommand command)
        {
            return new CompanyEntity()
            {
                Id = command.Id,
                Address = command.Address,
                Name = command.Name
            };
        }

        public static CompanyDto ToDto(this CompanyEntity companyEntity)
        {
            return new CompanyDto
            {
                Id = companyEntity.Id,
                Address = companyEntity.Address,
                Name = companyEntity.Name,
            };
        }
    }
}
