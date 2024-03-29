﻿using NEAT.Utils;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace NEAT
{
    public partial class game2048 : Form
    {
        private int width, height, startHeight, startWidth;
        public int score, best = 0, timeElapsed;
        private int[,] gameField = new int[4,4];
        private Random rnd = new Random();
        public bool background = true;

        private static System.Timers.Timer timer;

        public GameState state;

        public enum Direction { Up, Down, Left, Right };
        public enum GameState { Playing, GameOver };

        private Direction _prevMove;
        private int _sameMoves = 0;
        private int[,] _prevField = new int[4, 4];

        public game2048()
        {
            InitializeComponent();
        }

        private void game2048_Load(object sender, EventArgs e)
        {
            load();
        }

        public void load()
        {
            width = 500;
            height = 550;
            startHeight = 50;
            startWidth = 0;

            pbGameScreen.Size = new Size(width, height);
            pbGameScreen.Location = new Point(0, 0);

            initGame();

            draw();

            InfoManager.addLine("2048 loaded");
        }

        public void initGame()
        {
            gameField = new int[4, 4];

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    gameField[i, j] = 1;

            randomPiece();
            randomPiece();

            score = 0;
            timeElapsed = 0;
            if(timer == null)
            { 
                timer = new System.Timers.Timer();
                timer.Interval = 1000;
                timer.Elapsed += OnTimedEvent;
                timer.AutoReset = true;
            }
            timer.Enabled = false;

            state = GameState.Playing;

            draw();
        }

        private void randomPiece()
        {
            while(true)
            {
                int x = rnd.Next(0, 4);
                int y = rnd.Next(0, 4);

                if (gameField[x, y] != 1)
                    continue;

                gameField[x, y] = rnd.Next(0, 10) == 0 ? 4 : 2;
                break;
            }
        }

        private void draw()
        {
            if (background) return;

            Bitmap bmp = new Bitmap(pbGameScreen.Width, pbGameScreen.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                StringFormat stringCenter = new StringFormat();
                stringCenter.Alignment = StringAlignment.Center;

                StringFormat stringRight = new StringFormat();
                stringRight.Alignment = StringAlignment.Far;

                // Draw Details
                Font font = new Font(
                    FontFamily.GenericSansSerif,
                    22,
                    FontStyle.Bold,
                    GraphicsUnit.Pixel
                );

                g.DrawString(
                    "SCORE",
                    font,
                    new SolidBrush(ColorTranslator.FromHtml("#776e65")),
                    new Point(0, 0)
                );

                g.DrawString(
                    score.ToString(),
                    font,
                    new SolidBrush(ColorTranslator.FromHtml("#776e65")),
                    new Point(0, 25)
                );

                g.DrawString(
                    "BEST",
                    font,
                    new SolidBrush(ColorTranslator.FromHtml("#776e65")),
                    new Point(250, 0),
                    stringCenter
                );

                g.DrawString(
                    best.ToString(),
                    font,
                    new SolidBrush(ColorTranslator.FromHtml("#776e65")),
                    new Point(250, 25),
                    stringCenter
                );

                g.DrawString(
                    "TIME",
                    font,
                    new SolidBrush(ColorTranslator.FromHtml("#776e65")),
                    new Point(500, 0),
                    stringRight
                );

                g.DrawString(
                    timeElapsedFormatted(),
                    font,
                    new SolidBrush(ColorTranslator.FromHtml("#776e65")),
                    new Point(500, 25),
                    stringRight
                );

                // Draw Grid
                Color gridColor = ColorTranslator.FromHtml("#bbada0");
                g.DrawLine(new Pen(gridColor, 5), startWidth + 0, startHeight, startWidth + 0, height);
                g.DrawLine(new Pen(gridColor, 5), startWidth + 125, startHeight, startWidth + 125, height);
                g.DrawLine(new Pen(gridColor, 5), startWidth + 250, startHeight, startWidth + 250, height);
                g.DrawLine(new Pen(gridColor, 5), startWidth + 375, startHeight, startWidth + 375, height);
                g.DrawLine(new Pen(gridColor, 5), startWidth + 500, startHeight, startWidth + 500, height);

                g.DrawLine(new Pen(gridColor, 5), startWidth, startHeight + 0, width, startHeight + 0);
                g.DrawLine(new Pen(gridColor, 5), startWidth, startHeight + 125, width, startHeight + 125);
                g.DrawLine(new Pen(gridColor, 5), startWidth, startHeight + 250, width, startHeight + 250);
                g.DrawLine(new Pen(gridColor, 5), startWidth, startHeight + 375, width, startHeight + 375);
                g.DrawLine(new Pen(gridColor, 5), startWidth, startHeight + 500, width, startHeight + 500);

                // Draw Field
                font = new Font(
                    FontFamily.GenericSansSerif,
                    36,
                    FontStyle.Bold,
                    GraphicsUnit.Pixel
                );

                stringCenter.LineAlignment = StringAlignment.Center;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int tile = gameField[j, i];
                        g.FillRectangle(
                            calculateFieldColor(tile),
                            new Rectangle(
                                new Point(startWidth + 3 + 125 * j, startHeight + 3 +125 * i),
                                new Size(120, 120)
                            )
                        );

                        if(tile != 1)
                        {
                            g.DrawString(
                                tile.ToString(),
                                font,
                                calculateTextColor(tile),
                                new Point(startWidth + 60 + 125 * j, startHeight + 65 + 125 * i),
                                stringCenter
                            );
                        }
                    }
                }

                if (state == GameState.GameOver)
                {
                    g.DrawString(
                        "Game Over",
                        font,
                        Brushes.DarkRed,
                        new Point(width / 2, height / 2),
                        stringCenter
                    );
                }
            }

            pbGameScreen.Image = bmp;
        }

        private string timeElapsedFormatted()
        {
            TimeSpan t = TimeSpan.FromSeconds(timeElapsed);

            return string.Format("{0:D2}:{1:D2}:{2:D2}",
                t.Hours,
                t.Minutes,
                t.Seconds,
                t.Milliseconds);
        }

        private Brush calculateTextColor(int val)
        {
            if (val > 4)
                return new SolidBrush(ColorTranslator.FromHtml("#FFF"));
            return new SolidBrush(ColorTranslator.FromHtml("#776e65"));
        }

        private void game2048_KeyDown(object sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;

            if (state != GameState.GameOver)
            {
                if (key == Keys.Up || key == Keys.W)
                    moveTiles(Direction.Up);
                if (key == Keys.Down || key == Keys.S)
                    moveTiles(Direction.Down);
                if (key == Keys.Left || key == Keys.A)
                    moveTiles(Direction.Left);
                if (key == Keys.Right || key == Keys.D)
                    moveTiles(Direction.Right);
            }

            if (key == Keys.R)
                initGame();
            if (key == Keys.F)
                draw();
        }

        public void moveTiles(Direction direction)
        {
            bool addTile = false;
            timer.Enabled = true;

            // REF: [GITHUB] jakowskidev 2048 movement algorithm logic
            // https://github.com/jakowskidev/u2048_Jakowski/blob/master/u2048/Game.cs
            switch (direction)
            {
                case Direction.Up:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            for (int k = j + 1; k < 4; k++)
                            {
                                if (gameField[i, k] == 1)
                                {
                                    continue;
                                }
                                else if (gameField[i, k] == gameField[i, j])
                                {
                                    gameField[i, j] *= 2;
                                    score += gameField[i, j];
                                    gameField[i, k] = 1;
                                    addTile = true;
                                    break;
                                }
                                else
                                {
                                    if (gameField[i, j] == 1 && gameField[i, k] != 1)
                                    {
                                        gameField[i, j] = gameField[i, k];
                                        gameField[i, k] = 1;
                                        j--;
                                        addTile = true;
                                        break;
                                    }
                                    else if (gameField[i, j] != 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case Direction.Down:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 3; j >= 0; j--)
                        {
                            for (int k = j - 1; k >= 0; k--)
                            {
                                if (gameField[i, k] == 1)
                                {
                                    continue;
                                }
                                else if (gameField[i, k] == gameField[i, j])
                                {
                                    gameField[i, j] *= 2;
                                    score += gameField[i, j];
                                    gameField[i, k] = 1;
                                    addTile = true;
                                    break;
                                }
                                else
                                {
                                    if (gameField[i, j] == 1 && gameField[i, k] != 1)
                                    {
                                        gameField[i, j] = gameField[i, k];
                                        gameField[i, k] = 1;
                                        j++;
                                        addTile = true;
                                        break;
                                    }
                                    else if (gameField[i, j] != 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case Direction.Left:
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            for (int k = i + 1; k < 4; k++)
                            {
                                if (gameField[k, j] == 1)
                                {
                                    continue;
                                }
                                else if (gameField[k, j] == gameField[i, j])
                                {
                                    gameField[i, j] *= 2;
                                    score += gameField[i, j];
                                    gameField[k, j] = 1;
                                    addTile = true;
                                    break;
                                }
                                else
                                {
                                    if (gameField[i, j] == 1 && gameField[k, j] != 1)
                                    {
                                        gameField[i, j] = gameField[k, j];
                                        gameField[k, j] = 1;
                                        i--;
                                        addTile = true;
                                        break;
                                    }
                                    else if (gameField[i, j] != 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case Direction.Right:
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 3; i >= 0; i--)
                        {
                            for (int k = i - 1; k >= 0; k--)
                            {
                                if (gameField[k, j] == 1)
                                {
                                    continue;
                                }
                                else if (gameField[k, j] == gameField[i, j])
                                {
                                    gameField[i, j] *= 2;
                                    score += gameField[i, j];
                                    gameField[k, j] = 1;
                                    addTile = true;
                                    break;
                                }
                                else
                                {
                                    if (gameField[i, j] == 1 && gameField[k, j] != 1)
                                    {
                                        gameField[i, j] = gameField[k, j];
                                        gameField[k, j] = 1;
                                        i++;
                                        addTile = true;
                                        break;
                                    }
                                    else if (gameField[i, j] != 1)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            if (addTile)
                randomPiece();

            if (score > best)
                best = score;

            if (_prevMove == direction && fieldSame())
                _sameMoves++;
            else
                _sameMoves = 0;

            checkGameOver();

            _prevMove = direction;
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    _prevField[x, y] = gameField[x, y];

            draw();
        }

        public bool fieldSame()
        {
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    if (_prevField[x, y] != gameField[x, y])
                        return false;

            return true;
        }

        public void checkGameOver()
        {
            if (_sameMoves < 5)
            { 
                for (int i = 0; i < 4; i++)
                { 
                    for (int j = 0; j < 4; j++)
                    { 
                        if (i - 1 >= 0)
                            if (gameField[i - 1, j] == gameField[i, j])
                                return;

                        if (i + 1 < 4)
                            if (gameField[i + 1, j] == gameField[i, j])
                                return;

                        if (j - 1 >= 0)
                            if (gameField[i, j - 1] == gameField[i, j])
                                return;

                        if (j + 1 < 4)
                            if (gameField[i, j + 1] == gameField[i, j])
                                return;

                        if (gameField[i, j] == 1)
                            return;
                    }
                }
            }

            timer.Enabled = false;
            state = GameState.GameOver;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            timeElapsed++;
            draw();
        }

        private Brush calculateFieldColor(int val)
        {
            if (val == 2048)
                return new SolidBrush(ColorTranslator.FromHtml("#edc22e"));
            if (val == 1024)
                return new SolidBrush(ColorTranslator.FromHtml("#edc53f"));
            if (val == 512)
                return new SolidBrush(ColorTranslator.FromHtml("#edc850"));
            if (val == 256)
                return new SolidBrush(ColorTranslator.FromHtml("#edcc61"));
            if (val == 128)
                return new SolidBrush(ColorTranslator.FromHtml("#edcf72"));
            if (val == 64)
                return new SolidBrush(ColorTranslator.FromHtml("#f65e3b"));
            if (val == 32)
                return new SolidBrush(ColorTranslator.FromHtml("#f67c5f"));
            if (val == 16)
                return new SolidBrush(ColorTranslator.FromHtml("#f59563"));
            if (val == 8)
                return new SolidBrush(ColorTranslator.FromHtml("#f2b179"));
            if (val == 4)
                return new SolidBrush(ColorTranslator.FromHtml("#ede0c8"));
            if(val== 2)
                return new SolidBrush(ColorTranslator.FromHtml("#eee4da"));
            if (val == 1)
                return new SolidBrush(ColorTranslator.FromHtml("#cdc1b4"));
            return new SolidBrush(ColorTranslator.FromHtml("#3c3a32"));
        }

        public double[] getInputs()
        {
            double[] inputs = new double[16];
            int count = 0;

            int highest = 0;
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    if (highest < gameField[x, y])
                        highest = gameField[x, y];

            for (int x = 0; x < 4; x++)
                for(int y = 0; y < 4; y++)
                {
                    inputs[count] = (float)Math.Log(gameField[x, y], 2) / (float)Math.Log(highest, 2);
                    count++;
                }

            return inputs;
        }
    }
}
