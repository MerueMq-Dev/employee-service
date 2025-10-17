using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Passport.Commands
{
    public record UpdatePassportCommand(int Id, string Type, string Number) : IRequest<PassportDto>;

    public class UpdateDepartmentHandler(
    ILogger<UpdateDepartmentHandler> logger,
    IValidator<UpdatePassportCommand> validator,
    IPassportRepository passportRepository
    ) : IRequestHandler<UpdatePassportCommand, PassportDto>
    {
        public async Task<PassportDto> Handle(UpdatePassportCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating passport with Id {PassportId}", command.Id);

            await validator.ValidateAndThrowAsync(command);

            PassportEntity passportToUpdate = command.ToEntity();
        
            PassportEntity? updatedPassportEntity = await passportRepository.UpdateAsync(passportToUpdate, cancellationToken);

            if (updatedPassportEntity is null)
            {
                throw new NotFoundException($"Passport with id {command.Id} does not exist");
            }

            logger.LogInformation("Passport with id {PassportIc} was updated", updatedPassportEntity.Id);

            return updatedPassportEntity.ToDto();
        }
    }
}