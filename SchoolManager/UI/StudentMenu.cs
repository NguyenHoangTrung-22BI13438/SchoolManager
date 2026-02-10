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
    public static class StudentMenu
    {
        public static void Run(SchoolAppContext app)
        {
            Show(app.Students, app.StudentRepo);
        }
        public static void Show(StudentService service, IStudentRepository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== STUDENT MANAGEMENT ===");
                Console.WriteLine("1. List all students");
                Console.WriteLine("2. View student by ID");
                Console.WriteLine("3. Add student");
                Console.WriteLine("4. Update student");
                Console.WriteLine("5. Delete student");
                Console.WriteLine("6. Change student class");
                Console.WriteLine("7. List students by class");
                Console.WriteLine("0. Back to main menu");
                Console.WriteLine();

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListStudents(repo);
                        break;
                    case "2":
                        ViewStudentById(repo);
                        break;
                    case "3":
                        AddStudent(service);
                        break;
                    case "4":
                        UpdateStudent(repo);
                        break;
                    case "5":
                        DeleteStudent(repo);
                        break;
                    case "6":
                        ChangeStudentClass(service);
                        break;
                    case "7":
                        ListStudentsByClass(service);
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

        // ========== ACTIONS ==========
        private static void ListStudents(IStudentRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== STUDENT LIST ===");

            var students = repo.GetAll();
            if (students.Count == 0)
            {
                Console.WriteLine("No students found.");
            }
            else
            {
                foreach (var s in students)
                {
                    Console.WriteLine(
                        $"ID: {s.Id} | Name: {s.Name} | ClassID: {s.ClassroomId} | Status: {s.Status}");
                }
            }

            Pause();
        }

        private static void ViewStudentById(IStudentRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== VIEW STUDENT ===");

            Console.Write("Student ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var student = repo.GetById(id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
            }
            else
            {
                Console.WriteLine($"ID: {student.Id}");
                Console.WriteLine($"Name: {student.Name}");
                Console.WriteLine($"ClassID: {student.ClassroomId}");
                Console.WriteLine($"Status: {student.Status}");
            }

            Pause();
        }

        private static void AddStudent(StudentService service)
        {
            Console.Clear();
            Console.WriteLine("=== ADD STUDENT ===");

            Console.Write("ID: ");
            var id = int.Parse(Console.ReadLine()!);

            Console.Write("Name: ");
            var name = Console.ReadLine()!;

            Console.Write("ClassID: ");
            var classId = int.Parse(Console.ReadLine()!);

            Console.Write("Status (1=In, 2=Graduated, 3=Outed): ");
            var status = (StudentStatus)int.Parse(Console.ReadLine()!);

            service.EnrollStudent(new Student
            {
                Id = id,
                Name = name,
                ClassroomId = classId,
                Status = status
            });

            Console.WriteLine("Student added successfully.");
            Pause();
        }

        private static void UpdateStudent(IStudentRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== UPDATE STUDENT ===");

            Console.Write("Student ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var student = repo.GetById(id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                Pause();
                return;
            }

            Console.WriteLine($"Current Name: {student.Name}");
            Console.Write("New Name (leave empty to keep current): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
                student.Name = name;

            Console.WriteLine($"Current ClassID: {student.ClassroomId}");
            Console.Write("New ClassID (leave empty to keep current): ");
            var classIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(classIdInput))
                student.ClassroomId = int.Parse(classIdInput);

            Console.WriteLine($"Current Status: {student.Status}");
            Console.Write("New Status (1=In, 2=Graduated, 3=Outed, leave empty to keep current): ");
            var statusInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(statusInput))
                student.Status = (StudentStatus)int.Parse(statusInput);

            repo.Update(student);
            Console.WriteLine("Student updated successfully.");
            Pause();
        }

        private static void DeleteStudent(IStudentRepository repo)
        {
            Console.Clear();
            Console.WriteLine("=== DELETE STUDENT ===");

            Console.Write("Student ID: ");
            var id = int.Parse(Console.ReadLine()!);

            var student = repo.GetById(id);
            if (student == null)
            {
                Console.WriteLine("Student not found.");
            }
            else
            {
                Console.Write($"Are you sure you want to delete '{student.Name}'? (y/n): ");
                var confirm = Console.ReadLine()?.ToLower();
                if (confirm == "y")
                {
                    repo.Delete(id);
                    Console.WriteLine("Student deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }

            Pause();
        }

        private static void ChangeStudentClass(StudentService service)
        {
            Console.Clear();
            Console.WriteLine("=== CHANGE STUDENT CLASS ===");

            Console.Write("Student ID: ");
            var studentId = int.Parse(Console.ReadLine()!);

            Console.Write("New Class ID: ");
            var newClassId = int.Parse(Console.ReadLine()!);

            try
            {
                service.ChangeClassroom(studentId, newClassId);
                Console.WriteLine("Student class changed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Pause();
        }

        private static void ListStudentsByClass(StudentService service)
        {
            Console.Clear();
            Console.WriteLine("=== STUDENTS BY CLASS ===");

            Console.Write("Class ID: ");
            var classId = int.Parse(Console.ReadLine()!);

            var students = service.GetStudentsByClassroom(classId);
            if (students.Count == 0)
            {
                Console.WriteLine("No students found in this class.");
            }
            else
            {
                foreach (var s in students)
                {
                    Console.WriteLine($"ID: {s.Id} | Name: {s.Name} | Status: {s.Status}");
                }
            }

            Pause();
        }

        // ========== SHARED ==========
        private static void Pause()
        {
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}