using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.Passport.Commands
{
    public record UpdatePassportCommand(int Id, string Type, string Number) : IRequest<PassportDto>;

    public class UpdateDepartmentHandler(
    IValidator<UpdatePassportCommand> validator,
    IPassportRepository passportRepository
    ) : IRequestHandler<UpdatePassportCommand, PassportDto>
    {
        public async Task<PassportDto> Handle(UpdatePassportCommand command, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(command);

            PassportEntity passportToUpdate = new()
            {
                Id = command.Id,
                Type = command.Type,
                Number = command.Number,              
            };
        
            PassportEntity? updatedPassportEntity = await passportRepository.UpdateAsync(passportToUpdate, cancellationToken);

            if (updatedPassportEntity is null)
            {
                throw new NotFoundException($"Passport with id {command.Id} does not exist");
            }

            PassportDto updatedPassport = new ()
            {
                Id = updatedPassportEntity.Id,
                Type = updatedPassportEntity.Type,
                Number = updatedPassportEntity.Number,
            };

            return updatedPassport;
        }
    }
}