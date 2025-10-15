using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Application.DTOs
{
    public record CompanyDto
    {
        public required int Id { get; set; }

        public required string Name { get; set; }

        public required string Address { get; set; }
    }
}
