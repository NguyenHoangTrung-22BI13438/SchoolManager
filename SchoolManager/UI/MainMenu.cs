using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.UI;
namespace SchoolManager.UI
{
    public static class MainMenu
    {
        public static void Run(SchoolAppContext app)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SCHOOL MANAGEMENT SYSTEM ===");
                Console.WriteLine("1. Student Management");
                Console.WriteLine("2. Teacher Management");
                Console.WriteLine("3. Subject Management");
                Console.WriteLine("4. Classroom Management");
                Console.WriteLine("5. Grade Management");
                Console.WriteLine("6. Student Academic Report");
                Console.WriteLine("0. Exit");

                switch (Console.ReadLine())
                {
                    case "1": StudentMenu.Run(app); break;
                    case "2": TeacherMenu.Run(app); break;
                    case "3": SubjectMenu.Run(app); break;
                    case "4": ClassroomMenu.Run(app); break;
                    case "5": GradeMenu.Run(app); break;
                    case "6": ReportMenu.Run(app); break;
                    case "0": return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
