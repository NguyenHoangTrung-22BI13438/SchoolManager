using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.UI;

public static class ConsoleHelper
{
    public static void Pause()
    {
        Console.WriteLine("\nPress any key...");
        Console.ReadKey();
    }

    public static void InvalidChoice()
    {
        Console.WriteLine("Invalid choice.");
        Pause();
    }

    public static int ReadInt(string prompt)
    {
        Console.Write(prompt);
        return int.Parse(Console.ReadLine()!);
    }
}

