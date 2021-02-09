using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //[Required]
        public string Description { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}
