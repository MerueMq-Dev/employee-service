using Dapper;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EmployeeManager.Infrastructure.Repositories
{
    public class DepartmentRepository(IDbConnection _db) : IDepartmentRepository
    {
        public async Task<DepartmentEntity> CreateAsync(DepartmentEntity entity, CancellationToken cancellationToken = default)
        {
            var sql = @"
                INSERT INTO departments (name, phone, company_id)
                VALUES (@Name, @Phone, @CompanyId)
                RETURNING id, name, phone, company_id AS CompanyId";

            DepartmentEntity createdComapy = await _db.QuerySingleAsync<DepartmentEntity>(sql,
            new
            {
                Name = entity.Name,
                Phone = entity.Phone,
                CompanyId = entity.CompanyId
            }
            );

            return createdComapy;
        }

        public async Task<DepartmentEntity?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            DELETE FROM departments 
            WHERE id = @Id
            RETURNING id, name, phone, company_id AS CompanyId";

            DepartmentEntity? deletedDepartment = await _db.QuerySingleOrDefaultAsync<DepartmentEntity>(
                sql,
                new { Id = id }
            );

            return deletedDepartment;
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT COUNT(1) FROM departments WHERE id = @Id;";
            var count = await _db.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count > 0;
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT COUNT(1) FROM departments WHERE name = @Name;";
            var count = await _db.ExecuteScalarAsync<int>(sql, new { Name = name });
            return count > 0;
        }

        public async Task<bool> ExistsByNameAsync(string name, int companyId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
        SELECT COUNT(1) 
        FROM departments 
        WHERE name = @Name AND company_id = @CompanyId;
    ";

            var count = await _db.ExecuteScalarAsync<int>(sql, new { Name = name, CompanyId = companyId });
            return count > 0;
        }

        public async Task<int> GetCompanyIdByDepartmentIdAsync(int departmentId, CancellationToken cancellationToken)
        {
            const string sql = "SELECT company_id FROM departments WHERE id = @Id";
            return await _db.QuerySingleAsync<int>(sql, new { Id = departmentId });
        }

        public async Task<IEnumerable<DepartmentEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, name, phone, company_id AS CompanyId
            FROM departments
            ORDER BY id;";

            return await _db.QueryAsync<DepartmentEntity>(sql);           
        }

        public async Task<DepartmentEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, name, phone, company_id AS CompanyId
            FROM departments
            WHERE id = @Id;";

            return await _db.QuerySingleOrDefaultAsync<DepartmentEntity>(sql, new { Id = id });
        }

        public async Task<DepartmentEntity?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, name, phone, company_id AS CompanyId
            FROM departments
            WHERE name = @Name;";

            return await _db.QuerySingleOrDefaultAsync<DepartmentEntity>(sql, new { Name = name});
        }

        public async Task<DepartmentEntity?> UpdateAsync(DepartmentEntity entity, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            UPDATE departments
            SET name = @Name, phone = @Phone, company_id = @CompanyId
            WHERE id = @Id
            RETURNING id, name, phone, company_id AS CompanyId;";

            DepartmentEntity? updatedDepartment = await _db
                .QuerySingleOrDefaultAsync<DepartmentEntity>(sql, new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Phone = entity.Phone,
                    CompanyId = entity.CompanyId
                });

            return updatedDepartment;
        }
    }
}
