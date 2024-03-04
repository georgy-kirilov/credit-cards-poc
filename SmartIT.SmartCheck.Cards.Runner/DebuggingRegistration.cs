using System.Diagnostics;

namespace SmartIT.SmartCheck.Cards.Runner;

public static class DebuggingRegistration
{
    public static void EnableDevelopmentDebugging(this WebApplicationBuilder builder)
    {
        if (!builder.Environment.IsDevelopment())
        {
            return;
        }

        bool waitForDebugger = builder.Configuration.GetValue<bool>("WAIT_FOR_DEBUGGER");

        if (!waitForDebugger)
        {
            return;
        }

        Console.WriteLine("Application has been started in debug mode. Waiting for a debugging process to be attached...");

        while (true)
        {
            if (Debugger.IsAttached)
            {
                break;
            }

            Thread.Sleep(1_500);
        }

        Console.WriteLine("Debugging process has been attached successfully!");
    }
}
