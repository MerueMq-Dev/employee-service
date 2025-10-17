using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Passport.Commands;
using EmployeeManager.Application.UseCases.Passport.Commands;
using EmployeeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Mappers
{
    public static class PassportMapper
    {
        public static PassportEntity ToEntity(this CreatePassportCommand command)
        {
            return new()
            {
                Type = command.Type,
                Number = command.Number,
            };
        }

        public static PassportEntity ToEntity(this UpdatePassportCommand command)
        {
            return new()
            {
                Id = command.Id,
                Type = command.Type,
                Number = command.Number,
            };
        }

        public static PassportDto ToDto(this PassportEntity entity)
        {
            return new()
            {
                Id = entity.Id,
                Type = entity.Type,
                Number = entity.Number,
            };
        }
    }
}
