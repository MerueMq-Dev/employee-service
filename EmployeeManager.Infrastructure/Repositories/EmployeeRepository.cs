using Dapper;
using EmployeeManager.Application.DTOs;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Domain.Entities;
using EmployeManager.Domain.Entities;
using System.Data;

namespace EmployeeManager.Infrastructure.Repositories
{
    public class EmployeeRepository(IDbConnection _db) : IEmployeeRepository
    {
        public async Task<EmployeeEntity> CreateAsync(EmployeeEntity entity,  CancellationToken cancellationToken = default)
        {
            var sql = @"
                INSERT INTO employees (name, surname, phone, department_id, passport_id)
                VALUES (@Name, @Surname, @Phone, @DepartmentId, @PassportId)
                RETURNING id, name, surname, phone, department_id AS DepartmentId, passport_id AS PassportId";

            EmployeeEntity createdComapy = await _db.QuerySingleAsync<EmployeeEntity>(sql,
            new
            {
                Name = entity.Name,
                Surname = entity.Surname,
                Phone = entity.Phone,
                DepartmentId = entity.DepartmentId,
                PassportId = entity.PassportId
            }            
            );

            return createdComapy;
        }

        public async Task<EmployeeEntity?> DeleteAsync(int id,  CancellationToken cancellationToken = default)
        {
            const string sql = @"
            DELETE FROM employees 
            WHERE id = @Id
            RETURNING id, name, surname, phone, department_id AS DepartmentId, passport_id AS PassportId";

            EmployeeEntity? deletedDepartment = await _db.QuerySingleOrDefaultAsync<EmployeeEntity>(
                sql,
                new { Id = id }
            );

            return deletedDepartment;
        }

        public async Task<bool> ExistsAsync(int id,  CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT COUNT(1) FROM employees WHERE id = @Id;";
            var count = await _db.ExecuteScalarAsync<int>(sql, new { Id = id } );
            return count > 0;
        }

        public async Task<IEnumerable<EmployeeEntity>> GetAllAsync( CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, name, surname, phone, department_id AS DepartmentId, passport_id AS PassportId 
            FROM employees
            ORDER BY id;";

            return await _db.QueryAsync<EmployeeEntity>(sql);
        }
       
        public async Task<EmployeeEntity?> GetByIdAsync(int id,  CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, name, surname, phone, department_id AS DepartmentId, passport_id AS PassportId
            FROM employees
            WHERE id = @Id;";

            return await _db.QuerySingleOrDefaultAsync<EmployeeEntity>(sql, new { Id = id });
        }
    
        public async Task<EmployeeEntity?> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT id, name, surname, phone, department_id AS DepartmentId, passport_id AS PassportId
            FROM employees
            WHERE phone = @Phone;";

            return await _db.QuerySingleOrDefaultAsync<EmployeeEntity>(sql, new { Phone = phone });
        }
        public async Task<IEnumerable<EmployeeWithDetailsEntity>> GetAllEmployeeWithDetailsAsync(CancellationToken cancellation)
        {
            var sql = @"
            SELECT 
                e.id, e.name, e.surname, e.phone,
                d.id AS DId, d.name, d.phone, d.company_id AS CId,
                p.id AS PassportId, p.type, p.number
            FROM employees e
            INNER JOIN departments d ON e.department_id = d.id
            LEFT JOIN passports p ON e.passport_id = p.id            
        ";

            var employees = await _db.QueryAsync<EmployeeWithDetailsEntity, Department, Passport, EmployeeWithDetailsEntity>(
                sql,
                (emp, dept, pass) =>
                {
                    emp.Department = dept;
                    emp.CompanyId = dept.CId; // подтянем company_id
                    emp.Passport = pass;
                    return emp;
                },
                splitOn: "DId,PassportId"
            );

            return employees;
        }

        public async Task<EmployeeWithDetailsEntity> GetEmployeeWithDetailsByIdAsync(int id, CancellationToken cancellation)
        {
            var sql = @"
            SELECT 
                e.id, e.name, e.surname, e.phone,
                d.id AS DId, d.name, d.phone, d.company_id AS CId,
                p.id AS PassportId, p.type, p.number
            FROM employees e
            INNER JOIN departments d ON e.department_id = d.id
            LEFT JOIN passports p ON e.passport_id = p.id
            WHERE e.id = @Id
            LIMIT 1;
        ";

            var employees = await _db.QueryAsync<EmployeeWithDetailsEntity, Department, Passport, EmployeeWithDetailsEntity>(
                sql,
                (emp, dept, pass) =>
                {
                    emp.Department = dept;
                    emp.CompanyId = dept.CId; // подтянем company_id
                    emp.Passport = pass;
                    return emp;
                },
                new { Id = id },
                splitOn: "DId,PassportId"
            );

            return employees.FirstOrDefault()!;
        }

        public async Task<IEnumerable<EmployeeWithDetailsEntity>> GetEmployeeWithDetailsByCompanyIdAsync(int companyId, CancellationToken cancellation)
        {
            var sql = @"
            SELECT 
                e.id, e.name, e.surname, e.phone,
                d.id AS DId, d.name, d.phone, d.company_id AS CId,
                p.id AS PassportId, p.type, p.number
            FROM employees e
            INNER JOIN departments d ON e.department_id = d.id
            LEFT JOIN passports p ON e.passport_id = p.id
            WHERE d.company_id = @CompanyId;
        ";

            var employees = await _db.QueryAsync<EmployeeWithDetailsEntity, Department, Passport, EmployeeWithDetailsEntity>(
                sql,
                (emp, dept, pass) =>
                {
                    emp.Department = dept;
                    emp.CompanyId = dept.CId;
                    emp.Passport = pass;
                    return emp;
                },
                new { CompanyId = companyId },
                splitOn: "DId,PassportId"
            );

            return employees;
        }


        public async Task<IEnumerable<EmployeeWithDetailsEntity>> GetEmployeeWithDetailsByDepartmentIdAsync(int departmentId, CancellationToken cancellation)
        {
            var sql = @"
            SELECT 
                e.id, e.name, e.surname, e.phone,
                d.id AS DId, d.name, d.phone, d.company_id AS CId,
                p.id AS PassportId, p.type, p.number
            FROM employees e
            INNER JOIN departments d ON e.department_id = d.id
            LEFT JOIN passports p ON e.passport_id = p.id
            WHERE e.department_id = @DepartmentId;
        ";

            var employees = await _db.QueryAsync<EmployeeWithDetailsEntity, Department, Passport, EmployeeWithDetailsEntity>(
                sql,
                (emp, dept, pass) =>
                {
                    emp.Department = dept;
                    emp.CompanyId = dept.CId;
                    emp.Passport = pass;
                    return emp;
                },
                new { DepartmentId = departmentId },
                splitOn: "DId,PassportId"
            );

            return employees;
        }


        public async Task<EmployeeEntity?> UpdateAsync(EmployeeEntity entity,  CancellationToken cancellationToken = default)
        {
            const string sql = @"
            UPDATE employees
            SET name = @Name, surname = @Surname, phone = @Phone, department_id = @DepartmentId, passport_id = @PassportId
            WHERE id = @Id
            RETURNING id, name, surname, phone, department_id AS DepartmentId, passport_id AS PassportId;";

            EmployeeEntity? updatedDepartment = await _db
                .QuerySingleOrDefaultAsync<EmployeeEntity>(sql, new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Surname = entity.Surname,
                    Phone = entity.Phone,
                    DepartmentId = entity.DepartmentId,
                    PassportId = entity.PassportId
                });

            return updatedDepartment;
        }

        public async Task<EmployeeEntity?> GetByPassportIdAsync(int passportId, CancellationToken cancellationToken)
        {
            const string sql = @"
            SELECT id, name, surname, phone, department_id AS DepartmentId, passport_id AS PassportId
            FROM employees
            WHERE passport_id = @PassportId";

            return await _db.QuerySingleOrDefaultAsync<EmployeeEntity>(sql, new { PassportId = passportId });
        }
    }
}
