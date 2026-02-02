using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Domain.Rules
{
    public static class AcademicRules
    {
        public enum AcademicPerformance
        {
            Excellent,
            Good,
            Average,
            Poor
        }
        
        public static AcademicPerformance Evaluate(IReadOnlyList<Grade> grades)
        {
            if (grades == null || grades.Count == 0)
                throw new ArgumentException("Grades required");

            int excellentCount = grades.Count(g => g.Score >= 8.0m);
            bool anyBelow5 = grades.Any(g => g.Score < 5.0m);
            double avg = (double)grades.Average(g => g.Score);

            if (excellentCount >= 3 && !anyBelow5)
                return AcademicPerformance.Excellent;

            if (avg >= 7.0 && !anyBelow5)
                return AcademicPerformance.Good;

            int below5 = grades.Count(g => g.Score < 5.0m);
            bool anyBelow35 = grades.Any(g => g.Score < 3.5m);

            if (avg >= 5.0 && below5 <= 1 && !anyBelow35)
                return AcademicPerformance.Average;

            return AcademicPerformance.Poor;
        }
    }
}