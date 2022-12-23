using System;
using System.IO;

namespace minesweeper
{
    // + 1. чтобы звезды и цифры выводились как матрица, а не каждый раз с новой строки 
    // + 2. трай кетчи
    // + 3. оптимизация кода ( choosecell в старте уменьшать на -1) (( и ей подобные методы ))
    // + 4. Лишние методы и переменные 
    // + 5. красивое меню
    public class Game
    {
        private int m = 0;
        public int M
        {
            get { return m; }
            set { m = value; }
        }

        private int n = 0;
        public int N
        {
            get { return n; }
            set { N = value; }
        }

        private int amount = 0;

        private int result = 0;

        private Cell[,] cells;

        public void ShowField()
        {
            /* Console.Write("     ");
             for (int k = 0; k < m + 1; k++)
             {
                 Console.Write($"{k + 1}");
             }
             Console.WriteLine();*/
            for (int i = 0; i < n; i++)
            {
                if (i > 8)
                {
                    Console.Write($"{i + 1} | ");
                }
                else
                {
                    Console.Write($"{i + 1}  | ");
                }

                for (int j = 0; j < m; j++)
                {
                    if (cells[i, j].IsVisiable)
                    {
                        Console.WriteLine(cells[i, j].BombsAround);
                    }
                    else
                    {
                        Console.Write("*");
                    }
                }
                Console.WriteLine();
            }
        }

        public void RandCell(int numberX, int numberY)
        {
            Random rand = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if ((rand.Next() % 4) == 1)
                    {
                        cells[i, j].IsBomb = true;
                    }
                }
            }
            cells[numberX, numberY].IsBomb = false;
        }

        public void FirstChoose()
        {
            amount = 0;
            /* Console.Write("     ");
             for (int k = 0; k < m+1; k++)
             {
                 Console.Write($"{k + 1}");
             }
             Console.WriteLine();*/
            for (int i = 0; i < n; i++)
            {
                if (i > 8)
                {
                    Console.Write($"{i + 1} | ");
                }
                else
                {
                    Console.Write($"{i + 1}  | ");
                }

                for (int j = 0; j < m; j++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Введите координаты первого хода");
            int numberX = int.Parse(Console.ReadLine());
            int numberY = int.Parse(Console.ReadLine());
            if (numberX <= 0 || numberY <= 0)
            {
                Console.WriteLine("Значения должны быть больше нуля");
                AfterStart();
            }
            cells = new Cell[n, m];
            RandCell(numberX, numberY);
            cells[numberX, numberY].IsVisiable = true;
            cells[numberX, numberY].BombsAround = CounterBombNear(numberX, numberY);
            amount++;
        }

        public void ChooseCell(int numberX, int numberY)
        {
            if (cells[numberX, numberY].Peek())
            {
                Console.WriteLine($"Вы проиграли! Ваш результат: {amount}");
                ReadResult();
                if (amount > result)
                {
                    SaveResult();
                    Console.WriteLine("Новый рекорд был сохранен");
                }
                else
                {
                    Console.WriteLine("Вы не побили рекорд");
                }
                Console.WriteLine("Введите Y чтобы начать сначала!");
                string restart = Console.ReadLine();
                if (restart == "Y")
                {
                    Start();
                }
                else
                {

                    Console.WriteLine("Удачи!");
                    System.Environment.Exit(0);
                }
            }
            else
            {
                cells[numberX, numberY].BombsAround = CounterBombNear(numberX, numberY);
            }

        }

        public int CounterBombNear(int numberX, int numberY)
        {
            int i = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (cells[x + numberX, y + numberY].IsBomb)
                    {
                        i++;
                    }
                }
            }
            return i;
        }
        public void AfterStart()
        {
            ChooseDifficulty();
            FirstChoose();
            while (true)
            {
                ShowField();
                Console.WriteLine("Введите координаты хода");
                int n2 = int.Parse(Console.ReadLine());
                int m2 = int.Parse(Console.ReadLine());
                if (n2 <= 0 || m2 <= 0)
                {
                    Console.WriteLine("Значения должны быть больше нуля");
                    AfterStart();
                }
                ChooseCell(n2, m2);
                amount++;
            }
        }
        public void ChooseDifficulty()
        {
            Console.WriteLine("Выберите уровень сложности: \n1. Легкий\n2. Нормальный\n3. Сложный\n4. Особый");
            int choise = int.Parse(Console.ReadLine());
            if (choise == 1)
            {
                n = 9;
                m = 9;
            }
            if (choise == 2)
            {
                n = 16;
                m = 16;
            }
            if (choise == 3)
            {
                n = 16;
                m = 30;
            }
            if (choise == 4)
            {
                Console.WriteLine("Bведите n");
                n = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите m");
                m = int.Parse(Console.ReadLine());
                if (n <= 0 || m <= 0)
                {
                    Console.WriteLine("Значения должны быть больше нуля");
                    ChooseDifficulty();
                }
            }
        }

        public void SaveResult()
        {
            File.WriteAllText("result.txt", amount.ToString());
        }

        public void ReadResult()
        {
            using (var streamReader = new StreamReader("result.txt"))
            {
                result += int.Parse(streamReader.ReadLine());
            }
        }

        public void Start()
        {
            Console.WriteLine("Выберите действие: \n1. Запуск игры\n2. Просмотр рекорда\n");
            int startChoise = int.Parse(Console.ReadLine());
            if (startChoise == 1)
            {
                AfterStart();
            }
            if (startChoise == 2)
            {
                ReadResult();
                Console.WriteLine($"Рекорд в игре: {result}");
                Start();
            }
            else
            {
                Console.WriteLine("Введите корректное значение");
                Start();
            }
        }
    }

    public class Cell
    {
        private bool isBomb = false;
        public bool IsBomb
        {
            get { return isBomb; }
            set { isBomb = value; }
        }

        private bool isVisiable = false;
        public bool IsVisiable
        {
            get { return isVisiable; }
            set { isVisiable = value; }
        }

        private int bombsAround = 0;
        public int BombsAround
        {
            get { return bombsAround; }
            set { bombsAround = value; }
        }

        public bool Peek()
        {
            isVisiable = true;
            return isBomb;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}