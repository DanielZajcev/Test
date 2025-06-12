using System;
using System.Collections.Generic;
using System.Threading;

namespace DotNet9Console
{
    internal class Program
    {
        static readonly int width = 40;
        static readonly int height = 20;
        static LinkedList<(int x, int y)> snake = new();
        static (int x, int y) direction = (1, 0);
        static (int x, int y) food;
        static readonly Random random = new();
        static bool running = true;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Clear();
            Console.SetWindowSize(width + 2, height + 2);
            Console.SetBufferSize(width + 2, height + 2);
            InitSnake();
            PlaceFood();

            var inputThread = new Thread(ReadInput) { IsBackground = true };
            inputThread.Start();

            while (running)
            {
                Update();
                Draw();
                Thread.Sleep(150);
            }

            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine("Game Over. Press any key to exit...");
            Console.ReadKey(true);
        }

        static void ReadInput()
        {
            while (running)
            {
                if (!Console.KeyAvailable)
                {
                    Thread.Sleep(10);
                    continue;
                }

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (direction != (0, 1)) direction = (0, -1);
                        break;
                    case ConsoleKey.DownArrow:
                        if (direction != (0, -1)) direction = (0, 1);
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction != (1, 0)) direction = (-1, 0);
                        break;
                    case ConsoleKey.RightArrow:
                        if (direction != (-1, 0)) direction = (1, 0);
                        break;
                }
            }
        }

        static void InitSnake()
        {
            snake.Clear();
            snake.AddFirst((width / 2, height / 2));
        }

        static void PlaceFood()
        {
            do
            {
                food = (random.Next(width), random.Next(height));
            } while (snake.Contains(food));
        }

        static void Update()
        {
            var head = snake.First.Value;
            var newHead = (head.x + direction.x, head.y + direction.y);

            if (newHead.x < 0 || newHead.x >= width ||
                newHead.y < 0 || newHead.y >= height ||
                snake.Contains(newHead))
            {
                running = false;
                return;
            }

            snake.AddFirst(newHead);
            if (newHead == food)
            {
                PlaceFood();
            }
            else
            {
                snake.RemoveLast();
            }
        }

        static void Draw()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (snake.Contains((x, y)))
                    {
                        Console.Write('O');
                    }
                    else if (food == (x, y))
                    {
                        Console.Write('*');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
