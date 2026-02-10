using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;

namespace SchoolManager.Application.Services
{
    public class GradeServices
    {
        private readonly IGradeRepository _grades;
        private readonly IStudentRepository _students;
        private readonly ISubjectRepository _subjects;

        public GradeServices(
            IGradeRepository grades,
            IStudentRepository students,
            ISubjectRepository subjects)
        {
            _grades = grades;
            _students = students;
            _subjects = subjects;
        }

        public void RecordGrade(int studentId, int subjectId, decimal score)
        {
            var student = _students.GetById(studentId)
                ?? throw new Exception("Student not found");

            var subject = _subjects.GetById(subjectId)
                ?? throw new Exception("Subject not found");

            if (score < 0 || score > 100)
                throw new ArgumentException("Score must be between 0 and 100");

            var grade = new Grade
            {
                StudentId = studentId,
                SubjectId = subjectId,
                Score = score
            };

            _grades.Add(grade);
        }

        public void UpdateGrade(int gradeId, decimal newScore)
        {
            var grade = _grades.GetById(gradeId)
                ?? throw new Exception("Grade not found");

            if (newScore < 0 || newScore > 100)
                throw new ArgumentException("Score must be between 0 and 100");

            grade.Score = newScore;
            _grades.Update(grade);
        }

        public IReadOnlyList<Grade> GetStudentGrades(int studentId)
        {
            var student = _students.GetById(studentId)
                ?? throw new Exception("Student not found");

            return _grades.GetByStudentId(studentId);
        }
    }
}
