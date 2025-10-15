using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.UseCases.Passport.Commands;

public record CreatePassportCommand : IRequest<PassportDto>
{
    public required string Type { get; set; }
    public required string Number { get; set; }
}


public class CreateDepartmentHandler(
    IValidator<CreatePassportCommand> validator,
    IPassportRepository passportRepository
    )
    : IRequestHandler<CreatePassportCommand, PassportDto>
{
    public async Task<PassportDto> Handle(
        CreatePassportCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command);

        bool passportExists = await passportRepository.ExistsByNumberAsync(command.Number);
        if (passportExists)
        {
            throw new BusinessException($"Passport with number {command.Number} alredy exists");
        }

        PassportEntity passportToCreate = new()
        {
            Type = command.Type,
            Number = command.Number,
        };

        PassportEntity createdPassportEntity = await passportRepository.CreateAsync(passportToCreate,
            cancellationToken);

        PassportDto createdPassport = new ()
        {
            Id = createdPassportEntity.Id,
            Type = createdPassportEntity.Type,
            Number = createdPassportEntity.Number,
        };

        return createdPassport;
    }
}