using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeeManager.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EmployeeManager.Application.UseCases.Passport.Queries
{
    public record GetPassportByIdQuery(int Id): IRequest<PassportDto>;


    public class GetCompanyByIdHandler(
        IValidator<GetPassportByIdQuery> validator,
        IPassportRepository passportRepository
        ) : IRequestHandler<GetPassportByIdQuery, PassportDto>
    {
        public async Task<PassportDto> Handle(GetPassportByIdQuery query, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(query);

            PassportEntity? passportEntity = await passportRepository.GetByIdAsync(query.Id, cancellationToken);

            if (passportEntity is null)
            {
                throw new NotFoundException($"Passport with id {query.Id} does not exist");
            }

            PassportDto foundPassport = new()
            {
                Id = passportEntity.Id,
                Type = passportEntity.Type,
                Number = passportEntity.Number
            };

            return foundPassport;
        }
    }

}
