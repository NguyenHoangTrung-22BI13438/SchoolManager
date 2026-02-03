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
    public static class SubjectMenu
    {
        public static void Run(SchoolAppContext app)
        {
            Show(app.Subjects, app.SubjectRepo, app.TeacherRepo);
        }
        public static void Show(SubjectService service, JsonSubjectRepository repo, JsonTeacherRepository teacherRepo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SUBJECT MANAGEMENT ===");
                Console.WriteLine("1. List all subjects");
                Console.WriteLine("2. View subject by ID");
                Console.WriteLine("3. Add subject");
                Console.WriteLine("4. Update subject");
                Console.WriteLine("5. Delete subject");
                Console.WriteLine("6. Assign teacher to subject");
                Console.WriteLine("0. Back to main menu");
                Console.WriteLine();

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListSubjects(repo, teacherRepo);
                        break;
                    case "2":
                        ViewSubjectById(repo, teacherRepo);
                        break;
                    case "3":
                        AddSubject(repo);
                        break;
                    case "4":
                        UpdateSubject(repo);
                        break;
                    case "5":
                        DeleteSubject(repo);
                        break;
                    case "6":
                        AssignTeacher(service);
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
        static void ListSubjects(JsonSubjectRepository repo, JsonTeacherRepository teacherRepo)
        {
            Console.Clear();
            Console.WriteLine("=== SUBJECT LIST ===");

            var subjects = repo.GetAll();
            if (subjects.Count == 0)
            {
                Console.WriteLine("No subjects found.");
            }
            else
            {
                foreach (var s in subjects)
                {
                    var teacher = teacherRepo.GetById(s.TeacherId);
                    var teacherName = teacher?.Name ?? "Not assigned";
                    Console.WriteLine($"ID: {s.Id} | Name: {s.Name} | Teacher: {teacherName}");
                }
            }

            Pause();
        }

        static void ViewSubjectById(JsonSubjectRepository repo, JsonTeacherRepository teacherRepo)
        {
            Console.Clear();
            Console.WriteLine("=== VIEW SUBJECT ===");

            Console.Write("Subject ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var subject = repo.GetById(id);
            if (subject == null)
            {
                Console.WriteLine("Subject not found.");
            }
            else
            {
                var teacher = teacherRepo.GetById(subject.TeacherId);
                Console.WriteLine($"ID: {subject.Id}");
                Console.WriteLine($"Name: {subject.Name}");
                Console.WriteLine($"Teacher ID: {subject.TeacherId}");
                Console.WriteLine($"Teacher Name: {teacher?.Name ?? "Not assigned"}");
            }

            Pause();
        }

        static void AddSubject(JsonSubjectRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== ADD SUBJECT ===");

            Console.Write("ID: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("Name: ");
            var name = Console.ReadLine()!;

            Console.Write("Teacher ID (0 for none): ");
            var teacherId = int.Parse(Console.ReadLine()!);

            repo.Add(new Subject
            {
                Id = id,
                Name = name,
                TeacherId = teacherId
            });

            Console.WriteLine("Subject added successfully.");
            Pause();
        }

        static void UpdateSubject(JsonSubjectRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE SUBJECT ===");

            Console.Write("Subject ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var subject = repo.GetById(id);
            if (subject == null)
            {
                Console.WriteLine("Subject not found.");
                Pause();
                return;
            }

            Console.WriteLine($"Current Name: {subject.Name}");
            Console.Write("New Name (leave empty to keep current): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                subject.Name = name;

            Console.WriteLine($"Current Teacher ID: {subject.TeacherId}");
            Console.Write("New Teacher ID (leave empty to keep current): ");
            var teacherIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(teacherIdInput))
                subject.TeacherId = int.Parse(teacherIdInput);

            repo.Update(subject);
            Console.WriteLine("Subject updated successfully.");
            Pause();
        }

        static void DeleteSubject(JsonSubjectRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== DELETE SUBJECT ===");

            Console.Write("Subject ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var subject = repo.GetById(id);
            if (subject == null)
            {
                Console.WriteLine("Subject not found.");
            }
            else
            {
                Console.Write($"Are you sure you want to delete '{subject.Name}'? (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();
                if (confirm == "y")
                {
                    repo.Delete(id);
                    Console.WriteLine("Subject deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }

            Pause();
        }

        static void AssignTeacher(SubjectService service)
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGN TEACHER TO SUBJECT ===");

            Console.Write("Subject ID: ");
            var subjectId = int.Parse(Console.ReadLine()!);

            Console.Write("Teacher ID: ");
            var teacherId = int.Parse(Console.ReadLine()!);

            try
            {
                service.AssignTeacher(subjectId, teacherId);
                Console.WriteLine("Teacher assigned successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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
