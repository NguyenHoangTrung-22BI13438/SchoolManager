using SchoolManager.Application.Services;

namespace SchoolManager.UI
{
    public static class MainMenu
    {
        public static void Run(SchoolAppContext app)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║   SCHOOL MANAGEMENT SYSTEM             ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                Console.WriteLine();
                Console.WriteLine("1. Student Management");
                Console.WriteLine("2. Teacher Management");
                Console.WriteLine("3. Classroom Management");
                Console.WriteLine("4. Subject Management");
                Console.WriteLine("5. Grade Management");
                Console.WriteLine("6. Classroom-Subject Assignments");
                Console.WriteLine("7. Reports & Analytics");
                Console.WriteLine("0. Exit");
                Console.WriteLine();
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StudentMenu.Run(app);
                        break;
                    case "2":
                        TeacherMenu.Run(app);
                        break;
                    case "3":
                        ClassroomMenu.Run(app);
                        break;
                    case "4":
                        SubjectMenu.Run(app);
                        break;
                    case "5":
                        GradeMenu.Run(app);
                        break;
                    case "6":
                        ClassroomSubjectMenu.Run(app);
                        break;
                    case "7":
                        ReportMenu.Run(app);
                        break;
                    case "0":
                        Console.WriteLine("\nThank you for using the School Management System!");
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}