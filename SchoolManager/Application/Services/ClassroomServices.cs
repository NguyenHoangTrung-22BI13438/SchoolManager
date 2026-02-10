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
        private readonly IClassroomRepository _classrooms;
        private readonly IStudentRepository _students;
        private readonly IClassroomSubjectRepository _classroomSubjects;

        public ClassroomService(
            IClassroomRepository classrooms,
            IStudentRepository students,
            IClassroomSubjectRepository classroomSubjects)
        {
            _classrooms = classrooms;
            _students = students;
            _classroomSubjects = classroomSubjects;
        }

        public IReadOnlyList<Student> GetStudents(int classroomId)
        {
            return _students.GetAll()
                .Where(s => s.ClassroomId == classroomId)
                .ToList();
        }

        public IReadOnlyList<ClassroomSubject> GetSubjects(int classroomId)
        {
            return _classroomSubjects.GetByClassroomId(classroomId);
        }

        public void AssignSubject(int classroomId, int subjectId, int teacherId)
        {
            var classroom = _classrooms.GetById(classroomId)
                ?? throw new Exception("Classroom not found");

            // Check if this subject is already assigned to this classroom
            var existing = _classroomSubjects.GetByClassroomId(classroomId)
                .FirstOrDefault(cs => cs.SubjectId == subjectId);

            if (existing != null)
                throw new Exception("Subject already assigned to this classroom");

            var assignment = new ClassroomSubject
            {
                ClassroomId = classroomId,
                SubjectId = subjectId,
                TeacherId = teacherId
            };

            _classroomSubjects.Add(assignment);
        }

        public void RemoveSubject(int classroomId, int subjectId)
        {
            var assignment = _classroomSubjects.GetByClassroomId(classroomId)
                .FirstOrDefault(cs => cs.SubjectId == subjectId)
                ?? throw new Exception("Subject assignment not found in this classroom");

            _classroomSubjects.Delete(assignment.Id);
        }
    }
}
