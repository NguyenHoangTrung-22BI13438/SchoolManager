using SchoolManager.Application.Services;
using SchoolManager.Domain.Models;
using SchoolManager.Infrastructure.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.UI
{
    public static class ClassroomMenu
    {
        public static void Run(SchoolAppContext app)
        {
            Show(app.Classrooms, app.ClassroomRepo, app.StudentRepo);
        }

        public static void Show(ClassroomService service, JsonClassroomRepository repo, JsonStudentRepository studentRepo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CLASSROOM MANAGEMENT ===");
                Console.WriteLine("1. List all classrooms");
                Console.WriteLine("2. View classroom by ID");
                Console.WriteLine("3. Add classroom");
                Console.WriteLine("4. Update classroom");
                Console.WriteLine("5. Delete classroom");
                Console.WriteLine("6. List classrooms by grade");
                Console.WriteLine("0. Back to main menu");
                Console.WriteLine();

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListClassrooms(repo, studentRepo);
                        break;
                    case "2":
                        ViewClassroomById(repo, studentRepo);
                        break;
                    case "3":
                        AddClassroom(repo);
                        break;
                    case "4":
                        UpdateClassroom(repo);
                        break;
                    case "5":
                        DeleteClassroom(repo);
                        break;
                    case "6":
                        ListClassroomsByGrade(repo);
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
        static void ListClassrooms(JsonClassroomRepository repo, JsonStudentRepository studentRepo)
        {
            Console.Clear();
            Console.WriteLine("=== CLASSROOM LIST ===");

            var classrooms = repo.GetAll();
            if (classrooms.Count == 0)
            {
                Console.WriteLine("No classrooms found.");
            }
            else
            {
                foreach (var c in classrooms)
                {
                    var studentCount = studentRepo.GetAll().Count(s => s.ClassId == c.Id);
                    Console.WriteLine($"ID: {c.Id} | GradeID: {c.GradeId} | Students: {studentCount}");
                }
            }

            Pause();
        }

        static void ViewClassroomById(JsonClassroomRepository repo, JsonStudentRepository studentRepo)
        {
            Console.Clear();
            Console.WriteLine("=== VIEW CLASSROOM ===");

            Console.Write("Classroom ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var classroom = repo.GetById(id);
            if (classroom == null)
            {
                Console.WriteLine("Classroom not found.");
            }
            else
            {
                var students = studentRepo.GetAll().Where(s => s.ClassId == classroom.Id).ToList();
                Console.WriteLine($"ID: {classroom.Id}");
                Console.WriteLine($"GradeID: {classroom.GradeId}");
                Console.WriteLine($"Number of Students: {students.Count}");

                if (students.Count > 0)
                {
                    Console.WriteLine("\nStudents:");
                    foreach (var s in students)
                    {
                        Console.WriteLine($"  - {s.Name} (ID: {s.Id})");
                    }
                }
            }

            Pause();
        }

        static void AddClassroom(JsonClassroomRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== ADD CLASSROOM ===");

            Console.Write("ID: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("Grade ID: ");
            var gradeId = int.Parse(Console.ReadLine()!);

            repo.Add(new Classroom
            {
                Id = id,
                GradeId = gradeId
            });

            Console.WriteLine("Classroom added successfully.");
            Pause();
        }

        static void UpdateClassroom(JsonClassroomRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE CLASSROOM ===");

            Console.Write("Classroom ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var classroom = repo.GetById(id);
            if (classroom == null)
            {
                Console.WriteLine("Classroom not found.");
                Pause();
                return;
            }

            Console.WriteLine($"Current Grade ID: {classroom.GradeId}");
            Console.Write("New Grade ID (leave empty to keep current): ");
            var gradeIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(gradeIdInput))
                classroom.GradeId = int.Parse(gradeIdInput);

            repo.Update(classroom);
            Console.WriteLine("Classroom updated successfully.");
            Pause();
        }

        static void DeleteClassroom(JsonClassroomRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== DELETE CLASSROOM ===");

            Console.Write("Classroom ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var classroom = repo.GetById(id);
            if (classroom == null)
            {
                Console.WriteLine("Classroom not found.");
            }
            else
            {
                Console.Write($"Are you sure you want to delete classroom {classroom.Id}? (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();
                if (confirm == "y")
                {
                    repo.Delete(id);
                    Console.WriteLine("Classroom deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }

            Pause();
        }

        static void ListClassroomsByGrade(JsonClassroomRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== CLASSROOMS BY GRADE ===");

            Console.Write("Grade ID: ");
            var gradeId = int.Parse(Console.ReadLine()!);

            var classrooms = repo.GetByGradeId(gradeId);
            if (classrooms.Count == 0)
            {
                Console.WriteLine("No classrooms found for this grade.");
            }
            else
            {
                foreach (var c in classrooms)
                {
                    Console.WriteLine($"ID: {c.Id} | GradeID: {c.GradeId}");
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
