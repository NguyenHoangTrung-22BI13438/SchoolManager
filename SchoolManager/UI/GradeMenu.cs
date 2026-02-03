using SchoolManager.Domain.Models;
using SchoolManager.Infrastructure.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.UI
{
    public static class GradeMenu
    {
        public static void Run(SchoolAppContext app)
        {
            Show(app.GradeRepo, app.StudentRepo, app.SubjectRepo);
        }
        public static void Show(JsonGradeRepository repo, JsonStudentRepository studentRepo, JsonSubjectRepository subjectRepo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GRADE MANAGEMENT ===");
                Console.WriteLine("1. List all grades");
                Console.WriteLine("2. View grade by ID");
                Console.WriteLine("3. Add grade");
                Console.WriteLine("4. Update grade");
                Console.WriteLine("5. Delete grade");
                Console.WriteLine("6. View grades by student");
                Console.WriteLine("0. Back to main menu");
                Console.WriteLine();

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListGrades(repo, studentRepo, subjectRepo);
                        break;
                    case "2":
                        ViewGradeById(repo, studentRepo, subjectRepo);
                        break;
                    case "3":
                        AddGrade(repo);
                        break;
                    case "4":
                        UpdateGrade(repo);
                        break;
                    case "5":
                        DeleteGrade(repo);
                        break;
                    case "6":
                        ViewGradesByStudent(repo, subjectRepo);
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
        static void ListGrades(JsonGradeRepository repo, JsonStudentRepository studentRepo, JsonSubjectRepository subjectRepo)
        {
            Console.Clear();
            Console.WriteLine("=== GRADE LIST ===");

            var grades = repo.GetAll();
            if (grades.Count == 0)
            {
                Console.WriteLine("No grades found.");
            }
            else
            {
                foreach (var g in grades)
                {
                    var student = studentRepo.GetById(g.StudentID);
                    var subject = subjectRepo.GetById(g.SubjectID);
                    Console.WriteLine($"ID: {g.Id} | Student: {student?.Name ?? "Unknown"} | Subject: {subject?.Name ?? "Unknown"} | Score: {g.Score}");
                }
            }

            Pause();
        }

        static void ViewGradeById(JsonGradeRepository repo, JsonStudentRepository studentRepo, JsonSubjectRepository subjectRepo)
        {
            Console.Clear();
            Console.WriteLine("=== VIEW GRADE ===");

            Console.Write("Grade ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var grade = repo.GetById(id);
            if (grade == null)
            {
                Console.WriteLine("Grade not found.");
            }
            else
            {
                var student = studentRepo.GetById(grade.StudentID);
                var subject = subjectRepo.GetById(grade.SubjectID);
                Console.WriteLine($"ID: {grade.Id}");
                Console.WriteLine($"Student: {student?.Name ?? "Unknown"} (ID: {grade.StudentID})");
                Console.WriteLine($"Subject: {subject?.Name ?? "Unknown"} (ID: {grade.SubjectID})");
                Console.WriteLine($"Score: {grade.Score}");
            }

            Pause();
        }

        static void AddGrade(JsonGradeRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== ADD GRADE ===");

            Console.Write("ID: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("Student ID: ");
            var studentId = int.Parse(Console.ReadLine()!);

            Console.Write("Subject ID: ");
            var subjectId = int.Parse(Console.ReadLine()!);

            Console.Write("Score: ");
            var score = decimal.Parse(Console.ReadLine()!);

            repo.Add(new Grade
            {
                Id = id,
                StudentID = studentId,
                SubjectID = subjectId,
                Score = score
            });

            Console.WriteLine("Grade added successfully.");
            Pause();
        }

        static void UpdateGrade(JsonGradeRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE GRADE ===");

            Console.Write("Grade ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var grade = repo.GetById(id);
            if (grade == null)
            {
                Console.WriteLine("Grade not found.");
                Pause();
                return;
            }

            Console.WriteLine($"Current Student ID: {grade.StudentID}");
            Console.Write("New Student ID (leave empty to keep current): ");
            var studentIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(studentIdInput))
                grade.StudentID = int.Parse(studentIdInput);

            Console.WriteLine($"Current Subject ID: {grade.SubjectID}");
            Console.Write("New Subject ID (leave empty to keep current): ");
            var subjectIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(subjectIdInput))
                grade.SubjectID = int.Parse(subjectIdInput);

            Console.WriteLine($"Current Score: {grade.Score}");
            Console.Write("New Score (leave empty to keep current): ");
            var scoreInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(scoreInput))
                grade.Score = decimal.Parse(scoreInput);

            repo.Update(grade);
            Console.WriteLine("Grade updated successfully.");
            Pause();
        }

        static void DeleteGrade(JsonGradeRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== DELETE GRADE ===");

            Console.Write("Grade ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var grade = repo.GetById(id);
            if (grade == null)
            {
                Console.WriteLine("Grade not found.");
            }
            else
            {
                Console.Write($"Are you sure you want to delete grade {grade.Id}? (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();
                if (confirm == "y")
                {
                    repo.Delete(id);
                    Console.WriteLine("Grade deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }

            Pause();
        }

        static void ViewGradesByStudent(JsonGradeRepository repo, JsonSubjectRepository subjectRepo)
        {
            Console.Clear();
            Console.WriteLine("=== STUDENT GRADES ===");

            Console.Write("Student ID: ");
            var studentId = int.Parse(Console.ReadLine()!);

            var grades = repo.GetAll().Where(g => g.StudentID == studentId).ToList();
            if (grades.Count == 0)
            {
                Console.WriteLine("No grades found for this student.");
            }
            else
            {
                foreach (var g in grades)
                {
                    var subject = subjectRepo.GetById(g.SubjectID);
                    Console.WriteLine($"Subject: {subject?.Name ?? "Unknown"} | Score: {g.Score}");
                }
            }

            Pause();
        }
        private static void Pause()
        {
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}
