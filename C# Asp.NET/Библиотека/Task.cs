using GeneratorLibrary;
using System;
using System.Collections.Generic;

namespace WebApplication1
{
    public class Task
        {
            public List<Polynomial> Polynomials { get; set; } = new List<Polynomial>();
            private static string[,] tasks = new string[4, 5]
            {
            { "Представьте в виде многочлена:","Преобразуйте в многочлен:","Разложите на множители:","Упростите выражение:", "Пользуясь формулой куба суммы или разности, преобразуйте в многочлен:"},
            { "Представьте в виде многочлена:","Преобразуйте в многочлен:","Разложите на множители:","Упростите выражение:","Упростите выражение:"},
            { "Представьте в виде многочлена:","Преобразуйте в многочлен:","Разложите на множители:","Упростите выражение:","Упростите выражение:"},
            { "","","","",""}
            };
            public string task { get; set; }
            Random rng;
            public Task(Random rng, int paragraph, int tasks, int id)
            {
                task = Task.tasks[paragraph - 1, id];
                this.rng = rng;
                for (int i = 0; i < tasks; i++)
                {
                    switch (paragraph)
                    {
                        case 1:
                            Polynomials.Add(Task1(id));
                            break;
                        case 2:
                            Polynomials.Add(Task2(id));
                            break;
                        case 3:
                            Polynomials.Add(Task3(id));
                            break;
                        case 4:
                            Polynomials.Add(Task4(id));
                            break;
                    }
                }
            }

            private Polynomial Task1(int task)
            {
                Symbol x = rng.NextS(false);
                Symbol y = rng.NextS(true);
                Symbol z = rng.NextS(true);
                while (x == y)
                    y = rng.NextS(true);
                while (x == z || y == z)
                    z = rng.NextS(true);

                if (task == 0)
                {
                    return (rng.Next(1, 6) * x + rng.Next(1, 6) * rng.NextSign() * y) ^ 2;
                }
                else if (task == 1)
                {
                    return (rng.NextR(1, 3, 5) * (Polynomial)x + rng.Next(1, 6) * rng.NextSign() * (Polynomial)y / z) ^ 2;
                }
                else
                if (task == 2)
                {
                    int a = rng.Next(1, 6);
                    int b = rng.Next(1, 6) * rng.NextSign();
                    return (x * a ^ 2) + 2 * a * b * x * y + (y * b ^ 2);
                }

                if (task == 3)
                {
                    int a = rng.Next(1, 6);
                    int b = rng.Next(1, 6) * rng.NextSign();
                    return ((x * a ^ 2) + 2 * a * b * x * y + (y * b ^ 2)) / (a * x + b * y);
                }
                else
                {
                    return (rng.Next(1, 6) * x + rng.Next(1, 6) * rng.NextSign() * y) ^ 3;
                }
            }

            private Polynomial Task2(int task)
            {
                Symbol x = rng.NextS(false);
                Symbol y = rng.NextS(true);
                Symbol z = rng.NextS(true);
                while (x == y)
                    y = rng.NextS(true);
                while (x == z || y == z)
                    z = rng.NextS(true);

                if (task == 0)
                {
                    int a = rng.Next(1, 6) * rng.NextSign();
                    int b = rng.Next(1, 6) * rng.NextSign();
                    return (a * x + b * y) * (a * x - b * y);
                }
                else if (task == 1)
                {
                    Polynomial a = rng.NextR(1, 3, 5) * rng.NextSign();
                    int b = rng.Next(1, 6) * rng.NextSign();
                    return (a * x + b * y / z) * (a * x - b * y / z);
                }
                else
                if (task == 2)
                {
                    Polynomial a = rng.NextR(1, 3, 5);
                    Polynomial b = rng.Next(1, 6);
                    return (a * x ^ 2) - (b * y / z ^ 2);
                }

                if (task == 3)
                {
                    int a = rng.Next(1, 6);
                    int b = rng.Next(1, 6) * rng.NextSign();
                    return ((x * a ^ 2) - (y * b ^ 2)) / (a * x - b * y) * rng.NextSign();
                }
                else
                {
                    Polynomial a = rng.Next(1, 4);
                    Polynomial b = rng.Next(1, 4) * rng.NextSign();
                    Polynomial c = rng.Next(2, 4) * rng.NextSign();
                    Polynomial d = rng.Next(2, 4);
                    return ((x * a ^ 2) * c - (y * b ^ 2) * c) / (a * d * x - b * d * y);
                }
            }
            private Polynomial Task3(int task)
            {
                Symbol x = rng.NextS(false);
                Symbol y = rng.NextS(true);
                Symbol z = rng.NextS(true);
                while (x == y)
                    y = rng.NextS(true);
                while (x == z || y == z)
                    z = rng.NextS(true);

                if (task == 0)
                {
                    int a = rng.Next(1, 5);
                    int b = rng.Next(1, 5);
                    return (a * x + b * y) * ((a * x^2) - a*b*x*y + (b * y^2));
                }
                else if (task == 1)
                {
                    int a = rng.Next(1, 5);
                    int b = rng.Next(1, 5);
                    return (a * x - b * y) * ((a * x ^ 2) + a * b * x * y + (b * y ^ 2));
                }
                else
                if (task == 2)
                {
                    Polynomial a = rng.Next(1, 6);
                    Polynomial b = rng.Next(1, 6) * rng.NextSign();
                    return (a * x ^ 3) + (b * y ^ 3);
                }

                if (task == 3)
                {
                    int a = rng.Next(1, 6);
                    int b = rng.Next(1, 6) * rng.NextSign();
                    return ((x * a ^ 3) + (y * b ^ 3)) / (a * x - b * y);
                }
                else
                {
                    int a = rng.Next(1, 6);
                    int b = rng.Next(1, 6) * rng.NextSign();
                    return ((x * a ^ 3) + (y * b ^ 3)) / ((a * x ^ 2) + a * b * x * y + (b * y ^ 2));
                }
            }
            private Polynomial Task4(int task)
            {
                return new Symbol("x");
            }
        
    }
}
