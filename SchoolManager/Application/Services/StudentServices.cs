using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Application.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _students;
        private readonly IClassroomRepository _classes;

        public StudentService(
            IStudentRepository students,
            IClassroomRepository classes)
        {
            _students = students;
            _classes = classes;
        }

        public void EnrollStudent(Student student)
        {
            _students.Add(student);
        }

        public void ChangeClass(int studentId, int newClassId)
        {
            var student = _students.GetById(studentId)
                ?? throw new Exception("Student not found");

            student.ClassId = newClassId;
            _students.Update(student);
        }

        public IReadOnlyList<Student> GetStudentsByClass(int classId)
        {
            return _students.GetAll().Where(s => s.ClassId == classId).ToList();
        }
    }

}