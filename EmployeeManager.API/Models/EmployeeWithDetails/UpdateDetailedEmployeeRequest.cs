namespace EmployeeManager.API.Models.EmployeeWithDetails
{
    public class UpdateDetailedEmployeeRequest
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public UpdateDepartmentDto? Department { get; set; }
        public UpdatePassportDto? Passport { get; set; }
    }

    public class UpdateDepartmentDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdatePassportDto
    {
        public string? Type { get; set; }
        public string? Number { get; set; }
    }
}
