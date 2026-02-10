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
            var grades = _grades.GetAll().Where(g => g.StudentID == studentId).ToList();
            return _academicRules.Evaluate(grades);  
        }
    }
}