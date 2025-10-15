namespace EmployeeManager.API.Models.Employee
{
    public record CreateEmployeeRequest(string Name, string Surname, string Phone, int DepartmentId, int? PassportId);
}
