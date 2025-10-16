using EmployeeManager.Domain.Entities;
using EmployeManager.Domain.Entities;

namespace EmployeeManager.Application.Interfaces
{
    public interface IEmployeeRepository : IRepository<EmployeeEntity>
    {
        public Task<EmployeeWithDetailsEntity> GetEmployeeWithDetailsByIdAsync(int id, CancellationToken cancellation);
        public Task<IEnumerable<EmployeeWithDetailsEntity>> GetEmployeeWithDetailsByCompanyIdAsync(int companyId, CancellationToken cancellation);

        public Task<IEnumerable<EmployeeWithDetailsEntity>> GetEmployeeWithDetailsByDepartmentIdAsync(int companyId, CancellationToken cancellation);

        public Task<IEnumerable<EmployeeWithDetailsEntity>> GetAllEmployeeWithDetailsAsync(CancellationToken cancellation);

        public Task<EmployeeEntity?> GetByPassportIdAsync(int passportId, CancellationToken cancellationToken);

        public Task<EmployeeEntity?> GetByPhoneAsync(string phone, CancellationToken cancellationToken);
    }
}
