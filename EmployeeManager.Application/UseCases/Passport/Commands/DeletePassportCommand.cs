using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.Passport.Commands
{
    public record DeletePassportCommand(int Id) : IRequest<PassportDto>;

    public class DeleteDepartmentHandler(
     IValidator<DeletePassportCommand> validator,
     IPassportRepository passportRepository
     ) : IRequestHandler<DeletePassportCommand, PassportDto>
    {
        public async Task<PassportDto> Handle(DeletePassportCommand command, CancellationToken cancellationToken)
        {

            await validator.ValidateAndThrowAsync(command);

            PassportEntity? deletedPassportEntity = await passportRepository
                .DeleteAsync(command.Id, cancellationToken);

            if (deletedPassportEntity is null)
            {
                throw new NotFoundException($"Passport with id {command.Id} does not exist");
            }

            PassportDto deletedPassport = new()
            {
                Id = deletedPassportEntity.Id,
                Type = deletedPassportEntity.Type,
                Number = deletedPassportEntity.Number,
            };

            return deletedPassport;
        }
    }
}
