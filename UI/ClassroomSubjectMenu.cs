using SchoolManager.Application.Services;
using SchoolManager.Application.Interfaces;

namespace SchoolManager.UI
{
    public static class ClassroomSubjectMenu
    {
        public static void Run(SchoolAppContext app)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== CLASSROOM-SUBJECT ASSIGNMENTS ===");
                Console.WriteLine("1. List all assignments");
                Console.WriteLine("2. View assignments by classroom");
                Console.WriteLine("3. View assignments by teacher");
                Console.WriteLine("4. View assignments by subject");
                Console.WriteLine("5. Assign subject to classroom");
                Console.WriteLine("6. Remove subject from classroom");
                Console.WriteLine("0. Back to main menu");
                Console.WriteLine();

                Console.Write("Choose: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListAllAssignments(app);
                        break;
                    case "2":
                        ViewAssignmentsByClassroom(app);
                        break;
                    case "3":
                        ViewAssignmentsByTeacher(app);
                        break;
                    case "4":
                        ViewAssignmentsBySubject(app);
                        break;
                    case "5":
                        AssignSubjectToClassroom(app);
                        break;
                    case "6":
                        RemoveSubjectFromClassroom(app);
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

        static void ListAllAssignments(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== ALL CLASSROOM-SUBJECT ASSIGNMENTS ===");

            var assignments = app.ClassroomSubjectRepo.GetAll();
            if (assignments.Count == 0)
            {
                Console.WriteLine("No assignments found.");
            }
            else
            {
                foreach (var a in assignments)
                {
                    var classroom = app.ClassroomRepo.GetById(a.ClassroomId);
                    var subject = app.SubjectRepo.GetById(a.SubjectId);
                    var teacher = app.TeacherRepo.GetById(a.TeacherId);
                    
                    Console.WriteLine($"ID: {a.Id} | Classroom: {classroom?.Name ?? "Unknown"} | " +
                                    $"Subject: {subject?.Name ?? "Unknown"} | " +
                                    $"Teacher: {teacher?.Name ?? "Unknown"}");
                }
            }

            Pause();
        }

        static void ViewAssignmentsByClassroom(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGNMENTS BY CLASSROOM ===");

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

            Console.WriteLine($"\nClassroom: {classroom.Name}");
            Console.WriteLine("Subjects taught:");
            
            var assignments = app.ClassroomSubjectRepo.GetByClassroomId(classroomId);
            if (assignments.Count == 0)
            {
                Console.WriteLine("  No subjects assigned.");
            }
            else
            {
                foreach (var a in assignments)
                {
                    var subject = app.SubjectRepo.GetById(a.SubjectId);
                    var teacher = app.TeacherRepo.GetById(a.TeacherId);
                    Console.WriteLine($"  - {subject?.Name ?? "Unknown"} (Teacher: {teacher?.Name ?? "Unknown"})");
                }
            }

            Pause();
        }

        static void ViewAssignmentsByTeacher(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGNMENTS BY TEACHER ===");

            Console.Write("Teacher ID: ");
            if (!int.TryParse(Console.ReadLine(), out int teacherId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            var teacher = app.TeacherRepo.GetById(teacherId);
            if (teacher == null)
            {
                Console.WriteLine("Teacher not found.");
                Pause();
                return;
            }

            Console.WriteLine($"\nTeacher: {teacher.Name}");
            Console.WriteLine("Teaching assignments:");
            
            var assignments = app.ClassroomSubjectRepo.GetByTeacherId(teacherId);
            if (assignments.Count == 0)
            {
                Console.WriteLine("  No assignments found.");
            }
            else
            {
                foreach (var a in assignments)
                {
                    var classroom = app.ClassroomRepo.GetById(a.ClassroomId);
                    var subject = app.SubjectRepo.GetById(a.SubjectId);
                    Console.WriteLine($"  - {subject?.Name ?? "Unknown"} in {classroom?.Name ?? "Unknown"}");
                }
            }

            Pause();
        }

        static void ViewAssignmentsBySubject(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGNMENTS BY SUBJECT ===");

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

            Console.WriteLine($"\nSubject: {subject.Name}");
            Console.WriteLine("Taught in:");
            
            var assignments = app.ClassroomSubjectRepo.GetBySubjectId(subjectId);
            if (assignments.Count == 0)
            {
                Console.WriteLine("  Not assigned to any classroom.");
            }
            else
            {
                foreach (var a in assignments)
                {
                    var classroom = app.ClassroomRepo.GetById(a.ClassroomId);
                    var teacher = app.TeacherRepo.GetById(a.TeacherId);
                    Console.WriteLine($"  - {classroom?.Name ?? "Unknown"} (Teacher: {teacher?.Name ?? "Unknown"})");
                }
            }

            Pause();
        }

        static void AssignSubjectToClassroom(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== ASSIGN SUBJECT TO CLASSROOM ===");

            Console.Write("Classroom ID: ");
            if (!int.TryParse(Console.ReadLine(), out int classroomId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            Console.Write("Subject ID: ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            Console.Write("Teacher ID: ");
            if (!int.TryParse(Console.ReadLine(), out int teacherId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            try
            {
                app.Classrooms.AssignSubject(classroomId, subjectId, teacherId);
                Console.WriteLine("Subject assigned successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Pause();
        }

        static void RemoveSubjectFromClassroom(SchoolAppContext app)
        {
            Console.Clear();
            Console.WriteLine("=== REMOVE SUBJECT FROM CLASSROOM ===");

            Console.Write("Classroom ID: ");
            if (!int.TryParse(Console.ReadLine(), out int classroomId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            Console.Write("Subject ID: ");
            if (!int.TryParse(Console.ReadLine(), out int subjectId))
            {
                Console.WriteLine("Invalid input.");
                Pause();
                return;
            }

            try
            {
                app.Classrooms.RemoveSubject(classroomId, subjectId);
                Console.WriteLine("Subject removed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
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