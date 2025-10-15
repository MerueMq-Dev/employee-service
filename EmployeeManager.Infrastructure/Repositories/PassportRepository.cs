using Dapper;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using System.Data;

namespace EmployeeManager.Infrastructure.Repositories
{
    public class PassportRepository : IPassportRepository
    {
        private readonly IDbConnection _db;

        public PassportRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<PassportEntity> CreateAsync(PassportEntity entity,  CancellationToken cancellationToken = default)
        {
            var sql = @"
                INSERT INTO passports (type, number)
                VALUES (@Type, @Number)
                RETURNING id, type, number";

            PassportEntity createdComapy = await _db.QuerySingleAsync<PassportEntity>(sql,
            new
            {               
                Type = entity.Type,
                Number = entity.Number,
            }
            );

            return createdComapy;
        }

        public async Task<PassportEntity?> DeleteAsync(int id,  CancellationToken cancellationToken = default)
        {
            const string sql = @"
            DELETE FROM passports
            WHERE id = @Id
            RETURNING id, type, number";

            PassportEntity? deletedDepartment = await _db.QuerySingleOrDefaultAsync<PassportEntity>(
                sql,
                new { Id = id }                
            );

            return deletedDepartment;
        }

        public async Task<bool> ExistsAsync(int id,  CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT COUNT(1) FROM passports WHERE id = @Id;";
            var count = await _db.ExecuteScalarAsync<int>(sql, new { Id = id });
            return count > 0;

        }

        public async Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT COUNT(1) FROM passports WHERE number = @Number;";
            var count = await _db.ExecuteScalarAsync<int>(sql, new { Number = number });
            return count > 0;          
        }

        public async Task<IEnumerable<PassportEntity>> GetAllAsync( CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, type, number
            FROM passports
            ORDER BY id;";

            return await _db.QueryAsync<PassportEntity>(sql);
        }

        public async Task<PassportEntity?> GetByIdAsync(int id,  CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, type, number
            FROM passports
            WHERE id = @Id;";

            return await _db.QuerySingleOrDefaultAsync<PassportEntity>(sql, new { Id = id });
        }

        public async Task<PassportEntity?> GetByNumberAsync(string number, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, type, number
            FROM passports
            WHERE number = @Number;";

            return await _db.QuerySingleOrDefaultAsync<PassportEntity>(sql, new { Number = number});
        }

        public async Task<PassportEntity?> UpdateAsync(PassportEntity entity,  CancellationToken cancellationToken = default)
        {
            const string sql = @"
            UPDATE passports
            SET type = @Type, number = @Number
            WHERE id = @Id
            RETURNING id, type, number;";

            PassportEntity? updatedDepartment = await _db
                .QuerySingleOrDefaultAsync<PassportEntity>(sql, new
                {
                    Id = entity.Id,
                    Type = entity.Type,
                    Number = entity.Number,
                });

            return updatedDepartment;
        }
    }
}