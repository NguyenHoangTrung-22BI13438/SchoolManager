using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Application.Services
{
    public class ReportService
    {
        private readonly IGradeRepository _grades;
        private readonly ISubjectRepository _subjects;
        private readonly IAcademicRules _academicRules;

        public ReportService(
            IGradeRepository grades,
            ISubjectRepository subjects,
            IAcademicRules academicRules)
        {
            _grades = grades;
            _subjects = subjects;
            _academicRules = academicRules;
        }

        public AcademicPerformance EvaluateStudent(int studentId)
        {
            var grades = _grades.GetByStudentId(studentId);

            if (grades.Count == 0)
                throw new Exception("No grades found for this student");

            return _academicRules.Evaluate(grades.ToList());
        }

        public StudentTranscript GetStudentTranscript(int studentId)
        {
            var grades = _grades.GetByStudentId(studentId);

            var gradeDetails = grades.Select(g =>
            {
                var subject = _subjects.GetById(g.SubjectId);
                return new GradeDetail
                {
                    SubjectId = g.SubjectId,
                    SubjectName = subject?.Name ?? "Unknown",
                    Score = g.Score
                };
            }).ToList();

            var performance = grades.Count > 0
                ? _academicRules.Evaluate(grades.ToList())
                : AcademicPerformance.Poor;

            return new StudentTranscript
            {
                StudentId = studentId,
                Grades = gradeDetails,
                AverageScore = grades.Count > 0 ? grades.Average(g => g.Score) : 0,
                Performance = performance
            };
        }
    }

    // Supporting DTOs
    public class StudentTranscript
    {
        public int StudentId { get; set; }
        public List<GradeDetail> Grades { get; set; } = new();
        public decimal AverageScore { get; set; }
        public AcademicPerformance Performance { get; set; }
    }

    public class GradeDetail
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = "";
        public decimal Score { get; set; }
    }
}