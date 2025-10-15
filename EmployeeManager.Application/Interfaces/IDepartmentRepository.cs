using EmployeeManager.Domain.Entities;
using System.Data;

namespace EmployeeManager.Application.Interfaces
{
    public interface IDepartmentRepository : IRepository<DepartmentEntity>
    {
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<DepartmentEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

        public Task<int> GetCompanyIdByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken);
        
    }
}
