using System;

namespace Tron
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Program.StartGame();
        }
        static void StartGame()
        {
            TronGame game = new TronGame();
            game.Run();
        }
    }
}
