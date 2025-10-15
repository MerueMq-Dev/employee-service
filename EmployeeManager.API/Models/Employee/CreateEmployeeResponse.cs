namespace EmployeeManager.API.Models.Employee
{
    public record CreateEmployeeResponse(int Id, string Name, string Surname, string Phone, int DepartmentId, int? PassportId);
}
