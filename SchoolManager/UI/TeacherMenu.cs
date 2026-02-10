using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.Domain.Models;
using SchoolManager.Application.Services;
using SchoolManager.Application.Interfaces;

namespace SchoolManager.UI
{
    public static class TeacherMenu
    {
        public static void Run(SchoolAppContext app)
        {
            Show(app.Teachers, app.TeacherRepo);
        }
        public static void Show(TeacherService service, ITeacherRepository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== TEACHER MANAGEMENT ===");
                Console.WriteLine("1. List all teachers");
                Console.WriteLine("2. View teacher by ID");
                Console.WriteLine("3. Add teacher");
                Console.WriteLine("4. Update teacher");
                Console.WriteLine("5. Delete teacher");
                Console.WriteLine("6. View teacher's subjects");
                Console.WriteLine("0. Back to main menu");
                Console.WriteLine();

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListTeachers(repo);
                        break;
                    case "2":
                        ViewTeacherById(repo);
                        break;
                    case "3":
                        AddTeacher(repo);
                        break;
                    case "4":
                        UpdateTeacher(repo);
                        break;
                    case "5":
                        DeleteTeacher(repo);
                        break;
                    case "6":
                        ViewTeacherSubjects(service);
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
        static void ListTeachers(ITeacherRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== TEACHER LIST ===");

            var teachers = repo.GetAll();
            if (teachers.Count == 0)
            {
                Console.WriteLine("No teachers found.");
            }
            else
            {
                foreach (var t in teachers)
                {
                    Console.WriteLine($"ID: {t.Id} | Name: {t.Name}");
                }
            }

            Pause();
        }

        static void ViewTeacherById(ITeacherRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== VIEW TEACHER ===");

            Console.Write("Teacher ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var teacher = repo.GetById(id);
            if (teacher == null)
            {
                Console.WriteLine("Teacher not found.");
            }
            else
            {
                Console.WriteLine($"ID: {teacher.Id}");
                Console.WriteLine($"Name: {teacher.Name}");
            }

            Pause();
        }

        static void AddTeacher(ITeacherRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== ADD TEACHER ===");

            Console.Write("ID: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("Name: ");
            var name = Console.ReadLine()!;

            repo.Add(new Teacher
            {
                Id = id,
                Name = name
            });

            Console.WriteLine("Teacher added successfully.");
            Pause();
        }

        static void UpdateTeacher(ITeacherRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE TEACHER ===");

            Console.Write("Teacher ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var teacher = repo.GetById(id);
            if (teacher == null)
            {
                Console.WriteLine("Teacher not found.");
                Pause();
                return;
            }

            Console.WriteLine($"Current Name: {teacher.Name}");
            Console.Write("New Name (leave empty to keep current): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                teacher.Name = name;

            repo.Update(teacher);
            Console.WriteLine("Teacher updated successfully.");
            Pause();
        }

        static void DeleteTeacher(ITeacherRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== DELETE TEACHER ===");

            Console.Write("Teacher ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var teacher = repo.GetById(id);
            if (teacher == null)
            {
                Console.WriteLine("Teacher not found.");
            }
            else
            {
                Console.Write($"Are you sure you want to delete '{teacher.Name}'? (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();
                if (confirm == "y")
                {
                    repo.Delete(id);
                    Console.WriteLine("Teacher deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }

            Pause();
        }

        static void ViewTeacherSubjects(TeacherService service)
        {
            Console.Clear();
            Console.WriteLine("=== TEACHER'S SUBJECTS ===");

            Console.Write("Teacher ID: ");
            var teacherId = int.Parse(Console.ReadLine()!);

            var subjects = service.GetSubjects(teacherId);
            if (subjects.Count == 0)
            {
                Console.WriteLine("No subjects found for this teacher.");
            }
            else
            {
                foreach (var s in subjects)
                {
                    Console.WriteLine(s.ToString());
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