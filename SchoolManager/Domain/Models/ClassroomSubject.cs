using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Domain.Models
{
    /// <summary>
    /// Junction entity representing the assignment of a subject to a classroom with a specific teacher.
    /// This entity solves the many-to-many relationship between Classroom, Subject, and Teacher.
    /// </summary>
    public class ClassroomSubject
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
    }
}
