using SchoolManager.Application.Services;
using SchoolManager.Domain.Models;
using SchoolManager.Application.Interfaces;

namespace SchoolManager.UI
{
    public static class ClassroomMenu
    {
        public static void Run(SchoolAppContext app)
        {
            Show(app.Classrooms, app.ClassroomRepo, app.StudentRepo);
        }

        public static void Show(ClassroomService service, IClassroomRepository repo, IStudentRepository studentRepo)
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
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }
        }

        static void ListClassrooms(IClassroomRepository repo, IStudentRepository studentRepo)
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
                    var studentCount = studentRepo.GetAll().Count(s => s.ClassroomId == c.Id);
                    Console.WriteLine($"ID: {c.Id} | Name: {c.Name} | Students: {studentCount}");
                }
            }

            Pause();
        }

        static void ViewClassroomById(IClassroomRepository repo, IStudentRepository studentRepo)
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
                var students = studentRepo.GetAll().Where(s => s.ClassroomId == classroom.Id).ToList();
                Console.WriteLine($"ID: {classroom.Id}");
                Console.WriteLine($"Name: {classroom.Name}");
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

        static void AddClassroom(IClassroomRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== ADD CLASSROOM ===");

            Console.Write("ID: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("Name (e.g., '10A', '11-Science'): ");
            var name = Console.ReadLine()!;

            repo.Add(new Classroom
            {
                Id = id,
                Name = name
            });

            Console.WriteLine("Classroom added successfully.");
            Pause();
        }

        static void UpdateClassroom(IClassroomRepository repo)
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

            Console.WriteLine($"Current Name: {classroom.Name}");
            Console.Write("New Name (leave empty to keep current): ");
            var nameInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nameInput))
                classroom.Name = nameInput;

            repo.Update(classroom);
            Console.WriteLine("Classroom updated successfully.");
            Pause();
        }

        static void DeleteClassroom(IClassroomRepository repo)
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
                Console.Write($"Are you sure you want to delete classroom '{classroom.Name}'? (y/n): ");
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

        private static void Pause()
        {
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}