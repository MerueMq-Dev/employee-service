using EmployeeManager.Domain.Entities;
using EmployeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Interfaces
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id,  CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T?> UpdateAsync(T entity,  CancellationToken cancellationToken = default);
        Task<T?> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    }
}
