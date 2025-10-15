using EmployeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Domain.Entities
{
    public class PassportEntity : IEntity
    {
        public int Id { get; set; }
        public required string Type { get; set; }
        public required string Number { get; set; }
    }
}