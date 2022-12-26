using System;
using System.IO;

namespace Plane
{
    internal class Program
    {
        /// <summary>
        /// Поле состоит из пустых символов 'O', граница поля состоит из '*', самолет игрока является символом '+', самолет противника это '-'.
        /// Если проверяемая ячейка является символом 'O', то она является пустой. Если проверяемая ячейка является символом '*', то она является границей поля. 
        /// Если является символом '+', то она является самолетом игрока. Если является символом '-', то она является самолетом противника.  
        /// Класс Game является классом, который создает игровое поле, а также проверяет ячейку на наличие самолета.
        /// Класс Plane является классом, который создает самолет игрока, а также отвечает за перемещение и стрельбу.
        /// Класс Enemy является классом, который создает самолеты противников, а также отвечает за удалие их.
        /// TODO: Самолет уворачивается от противника. Если игрок выходит за границу поля или врезается в противника, то игра заканчивается.
        ///  
        /// </summary>
        /// <param name="args"></param>
        /// 



        public class Game
        {
            private int n = 0;
            public int N
            {
                get { return n; }
                set { n = value; }
            }

            private int m = 0;
            public int M
            {
                get { return m; }
                set { m = value; }
            }

            private int enemyCol = 0;
            public int EnemyCol
            {
                get { return enemyCol; }
                set { enemyCol = value; }
            }

            private int cellX = 0;
            public int CellX
            {
                get { return cellX; }
                set { cellX = value; }
            }

            private int cellY = 0;
            public int CellY
            {
                get { return cellY; }
                set { cellY = value; }
            }


            private int amount;

            private int result;

            private char[,] cells;

            public void ChooseDifficulty()
            {
                Console.WriteLine("Выберите уровень сложности: \n1. Легкий\n2. Нормальный\n3. Сложный\n4. Особый");
                int difChoose = int.Parse(Console.ReadLine());
                if (difChoose == 1)
                {
                    n = 20;
                    m = 20;
                }
                if (difChoose == 2)
                {
                    n = 15;
                    m = 15;
                }
                if (difChoose == 3)
                {
                    n = 10;
                    m = 10;
                }
                if (difChoose == 4)
                {
                    Console.WriteLine("Bведите n");
                    n = int.Parse(Console.ReadLine());
                    Console.WriteLine("Bведите m");
                    m = int.Parse(Console.ReadLine());
                }
                cells = new char[n, m];
            }

            public void CreateField()
            {
                ChooseDifficulty();
                for (int k = 0; k < m + 2; k++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
                for (int i = 0; i < n; i++)
                {
                    Console.Write("*");
                    for (int j = 0; j < m; j++)
                    {
                        cells[i, j] = 'o';

                        Console.Write(cells[i, j]);
                    }
                    Console.Write("*");
                    Console.WriteLine();
                }
                for (int k = 0; k < m + 2; k++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();

            }

            public void AfterStart()
            {
                CreateField();
                CreatePlane();
                ShowField();
                while (true)
                {
                    Step();
                }
            }

            public void ShowField()
            {
                for (int k = 0; k < m + 2; k++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
                for (int i = 0; i < n; i++)
                {
                    Console.Write("*");
                    for (int j = 0; j < m; j++)
                    {
                        Console.Write(cells[i, j]);
                    }
                    Console.Write("*");
                    Console.WriteLine();
                }
                for (int k = 0; k < m + 2; k++)
                {
                    Console.Write("*");
                }
                Console.WriteLine();
            }

            public void EnemyFabric()
            {
                Random rand = new Random();
                enemyCol = rand.Next(0, m - 1);
                cells[0, enemyCol] = '-';
            }

            public void CreatePlane()
            {
                cellX = n - 1;
                cellY = 9;
                cells[cellX, cellY] = '+' ;
            }

            public void Move()
            {
                ConsoleKeyInfo keyinfo;
                keyinfo = Console.ReadKey();
                Console.WriteLine("\n");
                cells[cellX, cellY] = 'o';
                if (keyinfo.Key == ConsoleKey.LeftArrow)
                {
                    if (cells[cellX, cellY - 1] == 'o')
                    {
                        cellY = cellY - 1;
                        cells[cellX, cellY] = '+';
                    }
                    else
                    {
                        Loser();
                    }
                }
                if (keyinfo.Key == ConsoleKey.RightArrow)
                {
                    if (cells[cellX, cellY + 1] == 'o')
                    {
                        cellY = cellY + 1;
                        cells[cellX, cellY] = '+';
                    }
                    else
                    {
                        Loser();
                    }
                }
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    if (cells[cellX - 1, cellY] == 'o')
                    {
                        cellX = cellX - 1;
                        cells[cellX, cellY] = '+';
                    }
                    else
                    {
                        Loser();
                    }
                }
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    if (cellX < n-1)
                    {
                        if (cells[cellX + 1, cellY] == 'o')
                        {
                            cellX = cellX + 1;
                            cells[cellX, cellY] = '+';
                        }
                        else
                        {
                            Loser();
                        }
                    }
                    else if (cellX >= n - 1)
                    {
                        Console.WriteLine("Нельзя покидать поле боя");
                        Move();
                    }
                }
                if (keyinfo.Key == ConsoleKey.Spacebar)
                {
                    Shoot();
                    cells[cellX, cellY] = '+';
                }
            }

            public void Loser()
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

            public void Shoot()
            {
                for (int i = 0; i < m -1; i++)
                {
                    if (cells[i, cellY] == '-')
                    {
                        Console.WriteLine("Вы попали!");
                        cells[i, cellY] = 'o';
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            public void Step()
            {
                cells[cellX, cellY] = 'o';
                for (int i = 0; i < m - 1; i++)
                {
                    cells[n - 1, i] = 'o';
                }
                for (int i = n - 1; i > 0; --i)
                {
                    for (int j = m - 1; j > 0; --j)
                    {
                        if (cells[i - 1, j] == '-')
                        {
                            cells[i, j] = cells[i - 1, j];
                            cells[i - 1, j] = 'o';
                        }
                    }
                }
                EnemyFabric();
                Move();
                ShowField();
                amount++;
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
                    result = 0;
                    Start();
                }
                else
                {
                    Console.WriteLine("Введите корректное значение");
                    Start();
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

            /* public void ChangeColor()
             {

                 foreach (Char c in cells)
                 {
                     if (c == '+')
                     {
                         Console.ForegroundColor = System.ConsoleColor.Green;
                     }
                     if (c == '-')
                     {
                         Console.ForegroundColor = System.ConsoleColor.Red;
                     }
                 }
             }*/

        }


        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}
