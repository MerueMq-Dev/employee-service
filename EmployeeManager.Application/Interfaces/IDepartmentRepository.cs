using EmployeeManager.Domain.Entities;
using System.Data;

namespace EmployeeManager.Application.Interfaces
{
    public interface IDepartmentRepository : IRepository<DepartmentEntity>
    {
        Task<bool> ExistsByNameAndCompanyIdAsync(string name, int companyId, CancellationToken cancellationToken = default);

        Task<bool> ExistsByPhoneAndCompanyIdAsync(string phone, int companyId, CancellationToken cancellationToken);

        Task<DepartmentEntity?> GetByNameAndCompanyIdAsync(string name, int companyId, CancellationToken cancellationToken = default);

        public Task<int> GetCompanyIdByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken);
    }
}
