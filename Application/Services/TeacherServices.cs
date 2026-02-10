using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;

namespace SchoolManager.Application.Services
{
    public class TeacherService
    {
        private readonly ITeacherRepository _teachers;
        private readonly IClassroomSubjectRepository _classroomSubjects;

        public TeacherService(
            ITeacherRepository teachers,
            IClassroomSubjectRepository classroomSubjects)
        {
            _teachers = teachers;
            _classroomSubjects = classroomSubjects;
        }

        public IReadOnlyList<ClassroomSubject> GetAssignments(int teacherId)
        {
            var teacher = _teachers.GetById(teacherId)
                ?? throw new Exception("Teacher not found");

            return _classroomSubjects.GetByTeacherId(teacherId);
        }

        public IReadOnlyList<int> GetClassrooms(int teacherId)
        {
            return _classroomSubjects.GetByTeacherId(teacherId)
                .Select(cs => cs.ClassroomId)
                .Distinct()
                .ToList();
        }

        public IReadOnlyList<int> GetSubjects(int teacherId)
        {
            return _classroomSubjects.GetByTeacherId(teacherId)
                .Select(cs => cs.SubjectId)
                .Distinct()
                .ToList();
        }
    }
}