using EmployeeManager.API.Models.EmployeeWithDetails;

namespace EmployeeManager.API.Models.Employee
{
    public record CreateEmployeeWithDetailsRequest(string Name, string Surname, string Phone, int CompanyId, DepartmentDetail Department, PassportDetail Passport);
}
