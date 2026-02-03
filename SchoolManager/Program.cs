using SchoolManager.Application.Services;
using SchoolManager.Domain.Models;
using SchoolManager.Infrastructure.Json;
using System;
using SchoolManager;
using SchoolManager.UI;

class Program
{
    static void Main()
    {
        // Build application context (DI root)
        var app = AppBootstrapper.Build();

        // Start UI
        MainMenu.Run(app);
    }
}
