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
        private readonly IClassroomRepository _classrooms;

        public StudentService(
            IStudentRepository students,
            IClassroomRepository classrooms)
        {
            _students = students;
            _classrooms = classrooms;
        }

        public void EnrollStudent(Student student)
        {
            _students.Add(student);
        }

        public void ChangeClassroom(int studentId, int classroomId)
        {
            var student = _students.GetById(studentId)
                ?? throw new Exception("Student not found");

            var classroom = _classrooms.GetById(classroomId)
                ?? throw new Exception("Classroom not found");

            student.ClassroomId = classroomId;
            _students.Update(student);
        }

        public IReadOnlyList<Student> GetStudentsByClassroom(int classroomId)
        {
            return _students.GetAll()
                .Where(s => s.ClassroomId == classroomId)
                .ToList();
        }
    }
}