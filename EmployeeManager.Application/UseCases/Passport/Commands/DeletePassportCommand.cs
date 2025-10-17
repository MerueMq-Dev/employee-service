using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.Passport.Commands
{
    public record DeletePassportCommand(int Id) : IRequest<PassportDto>;

    public class DeleteDepartmentHandler(
     ILogger<DeleteDepartmentHandler> logger,
     IValidator<DeletePassportCommand> validator,
     IPassportRepository passportRepository
     ) : IRequestHandler<DeletePassportCommand, PassportDto>
    {
        public async Task<PassportDto> Handle(DeletePassportCommand command, CancellationToken cancellationToken)
        {

            logger.LogInformation("Deleting employee with id {PassportId}", command.Id);

            await validator.ValidateAndThrowAsync(command);

            PassportEntity? deletedPassportEntity = await passportRepository
                .DeleteAsync(command.Id, cancellationToken);

            if (deletedPassportEntity is null)
            {
                throw new NotFoundException($"Passport with id {command.Id} does not exist");
            }

            logger.LogInformation("Passport with id {PassportID} was deleted", deletedPassportEntity.Id);

            return deletedPassportEntity.ToDto();
        }
    }
}
