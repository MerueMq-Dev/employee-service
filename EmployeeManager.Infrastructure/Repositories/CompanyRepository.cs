using Dapper;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using System.Data;

namespace EmployeeManager.Infrastructure.Repositories
{
    public class CompanyRepository(IDbConnection _db) : ICompanyRepository
    {
        public async Task<CompanyEntity> CreateAsync(CompanyEntity company,
            CancellationToken cancellationToken = default)
        {

            var sql = @"
                INSERT INTO companies (name, address)
                VALUES (@Name, @Address)
                RETURNING id, name, address";

            CompanyEntity createdComapy = await _db.QuerySingleAsync<CompanyEntity>(sql,
                new
                {
                    Name = company.Name,
                    Address = company.Address,
                }
                );

            return createdComapy;
        }

        public async Task<CompanyEntity?> GetByIdAsync(int id,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, name, address
            FROM companies
            WHERE id = @Id;";

            return await _db.QuerySingleOrDefaultAsync<CompanyEntity>(sql, new { Id = id });
        }

        public async Task<IEnumerable<CompanyEntity>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, name, address
            FROM companies
            ORDER BY id;";

            return await _db.QueryAsync<CompanyEntity>(sql);
        }

        public async Task<CompanyEntity?> UpdateAsync(CompanyEntity company,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
            UPDATE companies
            SET name = @Name, address = @Address
            WHERE id = @Id
            RETURNING id, name, address;";

            CompanyEntity? updatedCompany = await _db
                .QuerySingleOrDefaultAsync<CompanyEntity>(sql, new
                {
                    Id = company.Id,
                    Name = company.Name,
                    Address = company.Address,
                });

            return updatedCompany;
        }

        public async Task<CompanyEntity?> DeleteAsync(int id,
            CancellationToken cancellationToken = default)
        {
            const string sql = @"
            DELETE FROM companies
            WHERE id = @Id
            RETURNING id, name, address";

            CompanyEntity? deletedCompany = await _db.QuerySingleOrDefaultAsync<CompanyEntity>(
                sql,
                new { Id = id }
            );

            return deletedCompany;
        }

        public async Task<bool> ExistsAsync(int id,
            CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT COUNT(1) FROM companies WHERE id = @Id;";
            var count = await _db.ExecuteScalarAsync<int>(sql, new { Id = id});
            return count > 0;
        }
    }
}
