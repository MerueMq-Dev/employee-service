using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeManager.Application.UseCases.Passport.Queries
{
    public record GetPassportByIdQuery(int Id): IRequest<PassportDto>;

    public class GetPassportByIdHandler(
        ILogger<GetPassportByIdHandler> logger,
        IValidator<GetPassportByIdQuery> validator,
        IPassportRepository passportRepository
        ) : IRequestHandler<GetPassportByIdQuery, PassportDto>
    {
        public async Task<PassportDto> Handle(GetPassportByIdQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching passport with id {PassportId}", query.Id);

            await validator.ValidateAndThrowAsync(query);

            PassportEntity? passportEntity = await passportRepository.GetByIdAsync(query.Id, cancellationToken);

            if (passportEntity is null)
            {
                throw new NotFoundException($"Passport with id {query.Id} does not exist");
            }

            logger.LogInformation("Passport with id {PassportId} was retrieved", passportEntity.Id);

            return passportEntity.ToDto();
        }
    }
}
