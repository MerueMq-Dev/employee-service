
namespace EmployeeManager.API.Models.EmployeeWithDetails
{
    public record EmployeeWithDetailsResponse(
        int Id, string Name, string Surname, string Phone, int CompanyId,
        DepartmentDetail Department, PassportDetail? Passport
        );

}
