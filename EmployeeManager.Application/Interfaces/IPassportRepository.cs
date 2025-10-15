using EmployeeManager.Domain.Entities;
using System.Data;

namespace EmployeeManager.Application.Interfaces
{
    public interface IPassportRepository : IRepository<PassportEntity>
    {
        Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default);

        Task<PassportEntity?> GetByNumberAsync(string number, CancellationToken cancellationToken = default);
    }
}
