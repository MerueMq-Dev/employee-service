using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.UseCases.Passport.Queries
{
    public record GetAllPassportsQuery(): IRequest<IEnumerable<PassportDto>>;

    public class GetAllPassportsHandler(IPassportRepository passportRepository) : IRequestHandler<GetAllPassportsQuery, IEnumerable<PassportDto>>
    {
        public async Task<IEnumerable<PassportDto>> Handle(GetAllPassportsQuery query, CancellationToken cancellationToken)
        {
            return (await passportRepository.GetAllAsync())
                .Select(p => new PassportDto { Id = p.Id, Type = p.Type, Number = p.Number });
        }
    }
}
