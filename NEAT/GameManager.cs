using System.Windows.Forms;

namespace NEAT
{
    public static class GameManager
    {
        private static game2048 game2048;
        private static gameSnake gameSnake;

        public static void load(string game)
        {
            close();

            InfoManager.addLine("Loading: " + game);

            switch(game)
            {
                case "2048":
                    game2048 = new game2048();
                    game2048.StartPosition = FormStartPosition.CenterScreen;
                    game2048.Show();
                    break;
                case "Snake":
                    gameSnake = new gameSnake();
                    gameSnake.StartPosition = FormStartPosition.CenterScreen;
                    gameSnake.Show();
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

            if (gameSnake != null)
            {
                InfoManager.addLine("Closing: Snake");

                gameSnake.Close();
                gameSnake = null;

                InfoManager.clearLine();
            }
        }
    }
}
