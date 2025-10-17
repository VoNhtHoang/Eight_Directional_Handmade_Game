using System;

namespace Knight
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}


// using var game = new MyGame.Game1();
// game.Run();
