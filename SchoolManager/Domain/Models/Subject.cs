using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Domain.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int TeacherId { get; set; }
    }
}
