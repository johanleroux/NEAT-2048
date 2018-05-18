using NEAT.Utils;
using System.Windows.Forms;

namespace NEAT
{
    public static class GameManager
    {
        private static game2048 game2048;

        public static void load(string game)
        {
            close();

            switch(game)
            {
                case "2048":
                    game2048 = new game2048();
                    game2048.StartPosition = FormStartPosition.CenterScreen;
                    game2048.background = false;
                    game2048.Show();
                    break;
            }
        }

        private static void close()
        {
            if (game2048 != null)
            {
                InfoManager.addLine("Closing: 2048");

                game2048.Close();
                game2048 = null;

                InfoManager.clearLine();
            }
        }
    }
}
