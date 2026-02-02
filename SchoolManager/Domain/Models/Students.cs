using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Domain.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int ClassId { get; set; }
        public StudentStatus Status { get; set; }
    }
}
