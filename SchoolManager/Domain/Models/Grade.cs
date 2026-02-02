using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Domain.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public decimal Score { get; set; }
    }
}