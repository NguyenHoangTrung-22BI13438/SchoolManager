using SchoolManager.Application.Services;
using SchoolManager.Domain.Models;
using SchoolManager.Application.Interfaces;

namespace SchoolManager.UI
{
    public static class SubjectMenu
    {
        public static void Run(SchoolAppContext app)
        {
            Show(app.Subjects, app.SubjectRepo);
        }

        public static void Show(SubjectService service, ISubjectRepository repo)
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
                Console.WriteLine("0. Back to main menu");
                Console.WriteLine();

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListSubjects(repo);
                        break;
                    case "2":
                        ViewSubjectById(repo);
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
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }
            }
        }

        static void ListSubjects(ISubjectRepository repo)
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
                    Console.WriteLine($"ID: {s.Id} | Name: {s.Name}");
                }
            }

            Pause();
        }

        static void ViewSubjectById(ISubjectRepository repo)
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
                Console.WriteLine($"ID: {subject.Id}");
                Console.WriteLine($"Name: {subject.Name}");
            }

            Pause();
        }

        static void AddSubject(ISubjectRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== ADD SUBJECT ===");

            Console.Write("ID: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("Name: ");
            var name = Console.ReadLine()!;

            repo.Add(new Subject
            {
                Id = id,
                Name = name
            });

            Console.WriteLine("Subject added successfully.");
            Pause();
        }

        static void UpdateSubject(ISubjectRepository repo)
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

            repo.Update(subject);
            Console.WriteLine("Subject updated successfully.");
            Pause();
        }

        static void DeleteSubject(ISubjectRepository repo)
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

        private static void Pause()
        {
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}