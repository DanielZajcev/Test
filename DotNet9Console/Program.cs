using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DotNet9Console
{
    internal class GameForm : Form
    {
        private const int CellSize = 20;
        private const int GridWidth = 20;
        private const int GridHeight = 20;

        private enum GameState { Title, Playing, GameOver }
        private GameState state = GameState.Title;

        private readonly List<Point> snake = new();
        private Point direction = new(1, 0);
        private Point food;
        private readonly Random random = new();
        private readonly Timer timer;

        private readonly Button startButton;
        private readonly Button exitButton;

        public GameForm()
        {
            Text = "Snake";
            ClientSize = new Size(GridWidth * CellSize, GridHeight * CellSize);
            DoubleBuffered = true;

            startButton = new Button { Text = "Start", Width = 80 };
            exitButton = new Button { Text = "Exit", Width = 80 };
            Controls.Add(startButton);
            Controls.Add(exitButton);

            startButton.Click += (s, e) => StartGame();
            exitButton.Click += (s, e) => Close();

            LayoutTitleScreen();

            timer = new Timer { Interval = 150 };
            timer.Tick += (s, e) => GameLoop();

            KeyDown += OnKeyDown;
        }

        private void LayoutTitleScreen()
        {
            startButton.Location = new Point((ClientSize.Width - startButton.Width) / 2, ClientSize.Height / 2 - 30);
            exitButton.Location = new Point((ClientSize.Width - exitButton.Width) / 2, ClientSize.Height / 2 + 10);
        }

        private void StartGame()
        {
            Controls.Remove(startButton);
            Controls.Remove(exitButton);
            state = GameState.Playing;
            snake.Clear();
            snake.Add(new Point(GridWidth / 2, GridHeight / 2));
            direction = new Point(1, 0);
            PlaceFood();
            timer.Start();
            Focus();
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            if (state != GameState.Playing) return;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (direction != new Point(0, 1)) direction = new Point(0, -1);
                    break;
                case Keys.Down:
                    if (direction != new Point(0, -1)) direction = new Point(0, 1);
                    break;
                case Keys.Left:
                    if (direction != new Point(1, 0)) direction = new Point(-1, 0);
                    break;
                case Keys.Right:
                    if (direction != new Point(-1, 0)) direction = new Point(1, 0);
                    break;
            }
        }

        private void GameLoop()
        {
            var head = snake[0];
            var newHead = new Point(head.X + direction.X, head.Y + direction.Y);

            if (newHead.X < 0 || newHead.X >= GridWidth ||
                newHead.Y < 0 || newHead.Y >= GridHeight ||
                snake.Contains(newHead))
            {
                timer.Stop();
                state = GameState.GameOver;
                Invalidate();
                return;
            }

            snake.Insert(0, newHead);
            if (newHead.Equals(food))
            {
                PlaceFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            Invalidate();
        }

        private void PlaceFood()
        {
            do
            {
                food = new Point(random.Next(GridWidth), random.Next(GridHeight));
            } while (snake.Contains(food));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.Clear(Color.Black);

            if (state == GameState.Title)
            {
                using var font = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold);
                var title = "Snake Game";
                var size = g.MeasureString(title, font);
                g.DrawString(title, font, Brushes.Lime, (ClientSize.Width - size.Width) / 2, ClientSize.Height / 4);
                return;
            }

            foreach (var p in snake)
            {
                g.FillRectangle(Brushes.Lime, p.X * CellSize, p.Y * CellSize, CellSize, CellSize);
            }
            g.FillEllipse(Brushes.Red, food.X * CellSize, food.Y * CellSize, CellSize, CellSize);

            if (state == GameState.GameOver)
            {
                using var font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                var text = "Game Over";
                var size = g.MeasureString(text, font);
                g.DrawString(text, font, Brushes.Yellow, (ClientSize.Width - size.Width) / 2, ClientSize.Height / 2);
            }
        }
    }

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new GameForm());
        }
    }
}
