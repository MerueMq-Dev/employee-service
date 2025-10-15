using EmployeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Domain.Entities
{
    public class EmployeeEntity : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public  required string Phone { get; set; }
        public required int DepartmentId { get; set; }
        public int? PassportId { get; set; }
    }
}
