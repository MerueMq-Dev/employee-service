namespace EmployeeManager.API.Models.Employee
{
    public record DeleteEmployeeResponse(int Id, string Name, string Surname, string Phone, int DepartmentId, int? PassportId);
}
