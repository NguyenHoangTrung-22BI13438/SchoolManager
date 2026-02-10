using SchoolManager.Application.Services;
using SchoolManager.Domain.Rules;

namespace SchoolManager.UI
{
    public static class ReportMenu
    {
        public static void Run(SchoolAppContext app)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== REPORTS & ANALYTICS ===");
                Console.WriteLine("1. Student Transcript");
                Console.WriteLine("2. Evaluate Student Performance");
                Console.WriteLine("3. Classroom Performance Summary");
                Console.WriteLine("4. Subject Statistics");
                Console.WriteLine("0. Back to main menu");
                Console.WriteLine();

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewStudentTranscript(app);
                        break;
                    case "2":
                        EvaluateStudent(app);
                        break;
                    case "3":
                        ClassroomPerformanceSummary(app);
                        break;
                    case "4":
                        SubjectStatistics(app);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }
        }

        static void ViewStudentTranscript(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== STUDENT TRANSCRIPT ===");

            Console.Write("Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            try
            {
                var student = app.StudentRepo.GetById(studentId);
                if (student == null)
                {
                    Console.WriteLine("Student not found.");
                    Pause();
                    return;
                }

                var transcript = app.Reports.GetStudentTranscript(studentId);

                Console.WriteLine($"\n╔════════════════════════════════════════╗");
                Console.WriteLine($"  Student: {student.Name}");
                Console.WriteLine($"  Student ID: {studentId}");
                Console.WriteLine($"╚════════════════════════════════════════╝");
                Console.WriteLine($"\nGrades:");
                
                if (transcript.Grades.Count == 0)
                {
                    Console.WriteLine("  No grades recorded.");
                }
                else
                {
                    foreach (var grade in transcript.Grades)
                    {
                        Console.WriteLine($"  {grade.SubjectName,-20} {grade.Score,6:F2}");
                    }
                    
                    Console.WriteLine($"\n{new string('─', 30)}");
                    Console.WriteLine($"Average Score: {transcript.AverageScore:F2}");
                    Console.WriteLine($"Performance:   {transcript.Performance}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Pause();
        }

        static void EvaluateStudent(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== EVALUATE STUDENT PERFORMANCE ===");

            Console.Write("Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            try
            {
                var student = app.StudentRepo.GetById(studentId);
                if (student == null)
                {
                    Console.WriteLine("Student not found.");
                    Pause();
                    return;
                }

                var performance = app.Reports.EvaluateStudent(studentId);
                
                Console.WriteLine($"\nStudent: {student.Name}");
                Console.WriteLine($"Academic Performance: {performance}");
                
                Console.WriteLine($"\nPerformance Criteria:");
                Console.WriteLine($"  - Excellent: Average >= 90");
                Console.WriteLine($"  - Good:      Average >= 75");
                Console.WriteLine($"  - Average:   Average >= 50");
                Console.WriteLine($"  - Poor:      Average < 50");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Pause();
        }

        static void ClassroomPerformanceSummary(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== CLASSROOM PERFORMANCE SUMMARY ===");

            Console.Write("Classroom ID: ");
            if (!int.TryParse(Console.ReadLine(), out int classroomId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            var classroom = app.ClassroomRepo.GetById(classroomId);
            if (classroom == null)
            {
                Console.WriteLine("Classroom not found.");
                Pause();
                return;
            }

            var students = app.Students.GetStudentsByClassroom(classroomId);
            
            Console.WriteLine($"\nClassroom: {classroom.Name}");
            Console.WriteLine($"Total Students: {students.Count}");
            Console.WriteLine($"\nStudent Performance:");

            if (students.Count == 0)
            {
                Console.WriteLine("  No students in this classroom.");
            }
            else
            {
                foreach (var student in students)
                {
                    try
                    {
                        var performance = app.Reports.EvaluateStudent(student.Id);
                        Console.WriteLine($"  {student.Name,-25} {performance}");
                    }
                    catch
                    {
                        Console.WriteLine($"  {student.Name,-25} No grades");
                    }
                }
            }

            Pause();
        }

        static void SubjectStatistics(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== SUBJECT STATISTICS ===");

            Console.Write("Subject ID: ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            var subject = app.SubjectRepo.GetById(subjectId);
            if (subject == null)
            {
                Console.WriteLine("Subject not found.");
                Pause();
                return;
            }

            var grades = app.GradeRepo.GetBySubjectId(subjectId);
            
            Console.WriteLine($"\nSubject: {subject.Name}");
            
            if (grades.Count == 0)
            {
                Console.WriteLine("No grades recorded for this subject.");
            }
            else
            {
                var scores = grades.Select(g => g.Score).ToList();
                var average = scores.Average();
                var highest = scores.Max();
                var lowest = scores.Min();
                
                Console.WriteLine($"Total Grades:   {grades.Count}");
                Console.WriteLine($"Average Score:  {average:F2}");
                Console.WriteLine($"Highest Score:  {highest:F2}");
                Console.WriteLine($"Lowest Score:   {lowest:F2}");
            }

            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}