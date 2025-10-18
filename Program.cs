using System;
using Serilog;

namespace Knight
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
            Log.Information("Running!!!");
            using (var game = new Knight.Game1())
                game.Run();
        }
    }
}

// using var game = new MyGame.Game1();
// game.Run();
