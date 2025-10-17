using EmployeeManager.Application.Department.Commands;
using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.UseCases.Company.Commands;
using EmployeeManager.Application.UseCases.Department.Commands;
using EmployeeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Mappers
{
    public static class DepartmentMappers
    {
        public static DepartmentEntity ToEntity(this CreateDepartmentCommand command)
        {
            return new()
            {
                Name = command.Name,
                Phone = command.Phone,
                CompanyId = command.CompanyId
            };
        }


        public static DepartmentEntity ToEntity(this UpdateDepartmentCommand command)
        {
            return new()
            {
                Id = command.Id,
                Name = command.Name,
                Phone = command.Phone,
                CompanyId = command.CompanyId
            };
        }


        public static DepartmentDto ToDto(this DepartmentEntity departmentEntity)
        {
            return new()
            {
                Id = departmentEntity.Id,
                Name = departmentEntity.Name,
                Phone = departmentEntity.Phone,
                CompanyId = departmentEntity.CompanyId
            };
        }
    }
}