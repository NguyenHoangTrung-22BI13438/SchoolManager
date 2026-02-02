using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Application.Services
{
    public class ClassroomService
    {
        private readonly IClassroomRepository _classes;
        private readonly IStudentRepository _students;

        public ClassroomService(
            IClassroomRepository classes,
            IStudentRepository students)
        {
            _classes = classes;
            _students = students;
        }

        public IReadOnlyList<Student> GetStudents(int classId)
        {
            return _students.GetAll().Where(s => s.ClassId == classId).ToList();
        }
    }
}
