using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Employee.Commands;
using EmployeeManager.Application.UseCases.Employee.Commands;
using EmployeeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeEntity ToEntity(this CreateEmployeeCommand command)
        {
            return new()
            {
                Name = command.Name,
                Surname = command.Surname,
                Phone = command.Phone,
                DepartmentId = command.DepartmentId,
                PassportId = command.PassportId
            };

        }

        public static EmployeeEntity ToEntity(this UpdateEmployeeCommand command)
        {
            return new()
            {
                Id = command.Id,
                Name = command.Name,
                Surname = command.Surname,
                Phone = command.Phone,
                DepartmentId = command.DepartmentId,
                PassportId = command.PassportId
            };

        }

        public static EmployeeDto ToDto(this EmployeeEntity employee)
        {
            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Phone = employee.Phone,
                DepartmentId = employee.DepartmentId,
                PassportId = employee.PassportId
            };

        }
    }
}
