using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;

namespace SchoolManager.Application.Services
{
    public class SubjectService
    {
        private readonly ISubjectRepository _subjects;
        private readonly IClassroomSubjectRepository _classroomSubjects;

        public SubjectService(
            ISubjectRepository subjects,
            IClassroomSubjectRepository classroomSubjects)
        {
            _subjects = subjects;
            _classroomSubjects = classroomSubjects;
        }

        public IReadOnlyList<int> GetClassrooms(int subjectId)
        {
            var subject = _subjects.GetById(subjectId)
                ?? throw new Exception("Subject not found");

            return _classroomSubjects.GetBySubjectId(subjectId)
                .Select(cs => cs.ClassroomId)
                .Distinct()
                .ToList();
        }

        public IReadOnlyList<int> GetTeachers(int subjectId)
        {
            return _classroomSubjects.GetBySubjectId(subjectId)
                .Select(cs => cs.TeacherId)
                .Distinct()
                .ToList();
        }
    }
}