using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Mappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.UseCases.Passport.Queries
{
    public record GetAllPassportsQuery(): IRequest<IEnumerable<PassportDto>>;

    public class GetAllPassportsHandler(
        ILogger<GetAllPassportsHandler> logger,
        IPassportRepository passportRepository
        ) : IRequestHandler<GetAllPassportsQuery, IEnumerable<PassportDto>>
    {
        public async Task<IEnumerable<PassportDto>> Handle(GetAllPassportsQuery query, CancellationToken cancellationToken)
        {
            logger.LogInformation("Fetching all passports");

            IEnumerable<PassportDto> passports = (await passportRepository.GetAllAsync())
                .Select(p => p.ToDto());

            logger.LogInformation("All passports were retrieved");

            return passports;

        }
    }
}
