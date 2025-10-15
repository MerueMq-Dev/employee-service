namespace EmployeeManager.Application.DTOs
{
    public record DepartmentDto()
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int CompanyId { get; set; }
    }
}
