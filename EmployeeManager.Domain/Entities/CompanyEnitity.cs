using EmployeManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManager.Domain.Entities
{
    //CompanyEntity
    public class CompanyEntity : IEntity
    {
        public int Id { get; set; }

        public required string Address { get; set; }

        public required string Name { get; set; }

    }
}
