using EmployeeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.Interfaces
{
    public interface ICompanyRepository: IRepository<CompanyEntity>
    {
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
