using SchoolManager.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.UI
{
    public static class ReportMenu
    {
        public static void Run(SchoolAppContext app)
        {
            Show(app.Reports);
        }
        public static void Show(ReportService service)
        {
            Console.Clear();
            Console.WriteLine("=== STUDENT ACADEMIC REPORT ===");

            Console.Write("Student ID: ");
            var id = int.Parse(Console.ReadLine()!);

            try
            {
                var result = service.EvaluateStudent(id);
                Console.WriteLine($"Academic Performance: {result}");
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
