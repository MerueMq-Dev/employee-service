using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.DTOs
{
    public class EmployeeWithDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }
        public DepartmentDetailsDto Department { get; set; }
        public PassportDetailsDto Passport { get; set; }
    }
}
