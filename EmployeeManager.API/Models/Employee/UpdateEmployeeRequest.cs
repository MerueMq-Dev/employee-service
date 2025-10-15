namespace EmployeeManager.API.Models.Employee
{
    public record UpdateEmployeeRequest(string Name, string Surname, string Phone, int DepartmentId, int? PassportId);
}
