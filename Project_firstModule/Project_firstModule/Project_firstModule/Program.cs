/*
* Дисциплина: "Программирование"
* Группа: 246ПИ
* Студент: Дулаев Арсен
* Дата: 2.10.2024
* Задача: Считать из файла input.txt массив, пропустив нечисловые значения.
* В числе С - посчитать сумму всех элементов с нечетными элементами, в P -
* произведение чисел, обратных каждому третьему.
*/
using System;
using System.Data;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Xml;

class Program
{
    /// <summary>
    /// Метод, отвечающий за получение входных данных для дальнейшего использования.
    /// </summary>
    /// <returns></returns>
    internal static string[] GetInput() 
    {
        try
        {
            string[] values = File.ReadAllLines("../../../WorkingFiles/input.txt"); // Указываем путь для файла с входными данными.
            return values;
        }
        catch (FileNotFoundException) // Если файла нет, сообщаем об этом пользователю и просим начать сначала.
        {
            Console.WriteLine("Проблемы с открытием файла.\nПопробуйте перезапустить решение и проверить наличие входного файла.");
            Environment.Exit(0);
            return [];
        }
    }
    /// <summary>
    /// Метод принимает на вход массив со строками входного файла и отсеивает некорректные значения.
    /// </summary>
    /// <param name="input">Массив со строками входного файла.</param>
    /// <returns></returns>
    internal static string[] SkipWrongTypes(string[] input)
    {
        int counter = 0;
        string[] X = new string[input.Length];
        foreach (string s in input)
        {
            string[] temp = s.Split(';'); // Отделяем данные друг от друга
            string filling = ""; // Строка, куда будем записывать данные, прошедшие "проверку" на корректность.
            if (temp.Length > 0) // Проверяем что данные есть.
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    if (int.TryParse(temp[i], out int a) && temp[i].Length > 0)
                    {

                        filling += $"{a} "; // Если целочисленные, то добавляем в нашу строку.
                    }
                    try
                    {
                        string y = filling.Remove(filling.Length - 1); // Удаляем лишний пробел в конце, для правильности вычислений.
                        X[counter] = y;
                    }
                    catch (ArgumentOutOfRangeException) // Если после фильтрации есть строка с длинной 0 - т.е пустая, не содержащая корректных данных.
                    {
                        X[counter] = "Корректных данных нет.";
                    }
                    }
            counter++;
            }
            else
            {
                return []; // "Заглушка", необходимая синтаксически, но не имеющая никакого влияния на выполнение программы (извините за такое).
            }
        }
        return X;
    }
    /// <summary>
    /// Метод конвертирующий массив строковых данных в массив целочисленных для дальнейших арифмпетических операций.
    /// </summary>
    /// <param name="input">Массив текстовых данных</param>
    /// <returns></returns>
    internal static int[][] ConvertToInt(string[] input)
    {
        int[][] X = new int[input.Length][]; // Создаем массив массивов для сохранения результата.
        int index = 0;
        foreach (string s in input)
        {
            try
            {
                int[] ints = s.Split(" ").Select(int.Parse).ToArray(); // Создаем из строки с числами массив с числами.
                X[index] = ints;
            }
            catch (FormatException) // Проверяем что все данные корректны. Если нет - завершаем программу.
            {
                Console.WriteLine("Корректных данных нет. Проверьте корректность входных данных.");
                Environment.Exit(0);
            }
            index++;
        }
        return X;
    }
    /// <summary>
    /// Подсчет числа P, выполненный согласно условию (произведение значений, обратных каждому третьему элементу массива).
    /// </summary>
    /// <param name="i">Массив целочисленных значений.</param>
    /// <returns></returns>
    internal static double[] CountP(int[][] i) 
    {
        double[] result = new double[i.Length];
        int index = 0;
        foreach (int[] ints in i)
        {
            double p = 1.0;
            for (int i2 = 0; i2 < ints.Length; i2++)
            {
                if (i2 % 3 == 0 && ints[i2] != 0)
                {
                    p *= Math.Round((double)1 / ints[i2], 3); // Считаем произведение значений, обратных каждому третьему элементу массива.
                }
                else if (ints[i2] == 0) // Проверяем не делим ли на ноль. Если делим - выводим ошибку и завершаем работу программы.
                {
                    Console.WriteLine("Произошло деление на 0. Текущая сессия была завершена");
                    Environment.Exit(0);
                }
            }
            result[index++] = p;
            
        }
        return result;
    }
    /// <summary>
    /// Подсчет числа C, выполненный согласно условию (сумма элементов с нечётными индексами).
    /// </summary>
    /// <param name="i">Массив целочисленных значений.</param>
    /// <returns></returns>
    internal static int[] CountC(int[][] i)
    {
        int[] results = new int[i.Length];
        int index = 0;
        foreach (int[] i2 in i)
        {
            int c = 0;
            for (int j = 0; j < i2.Length; j++)
            {
                c += (j % 2 == 1) ? i2[j] : 0; // Если индекс элемента нечётный - прибавляем его к итоговому, иначе не меняем сумму.
            }
            results[index++] = c;
        }
        return results;
    }
    /// <summary>
    /// Метод переписывает файл output.txt, содержащий выходные данные согласно формату вывода.
    /// </summary>
    /// <param name="C">Число C.</param>
    /// <param name="P">Число P.</param>
    /// <param name="number">Номер вызова программы.</param>
    internal static void RewriteOutput(int[] C, double[] P, string number)
    {
        File.WriteAllText($"../../../WorkingFiles/output-{number}.txt", ""); // Если файла нет - создаем. Если есть - очищаем.
        for (int i = 0; i < P.Length; i++)
        {
            File.AppendAllText($"../../../WorkingFiles/output-{number}.txt", $"{C[i]};{P[i]}\n"); // Вносим получившиеся результаты.
        }
    }
    /// <summary>
    /// Метод записывающий и получающий информацию из/в config.txt, с целью подсчёта количества запусков программы.
    /// </summary>
    /// <returns></returns>
    internal static string ConfigMaker()
    {
        try
        {
            string text = File.ReadAllText("../../../WorkingFiles/config.txt"); // Ищем файл конфига и "достаём" из него номер последнего созданного выходного файла.
            int num = int.Parse(text) + 1; // Увеличивем номер.
            File.WriteAllText("../../../WorkingFiles/config.txt", num.ToString()); // Записываем обновлённый номер.
            return num.ToString();
        }
        catch (FileNotFoundException)
        {
            File.WriteAllText("../../../WorkingFiles/config.txt", "1"); // Если файла конфига нет - создаём со значением "1" внутри.
            return "1";
        }
    }
    static void Main()
    {
        ConsoleKeyInfo keyToExit;
        do 
        {
            string[] input = GetInput(); // Читаем input.txt.
            string[] a = SkipWrongTypes(input); // Фильтруем от неподходящих значений.
            int[][] b = ConvertToInt(a); // Переводим данные в целочисленный тип данных.
            int[] c = CountC(b); // Считаем число C.
            double[] p = CountP(b); // Считаем число P.
            string generation = ConfigMaker(); // Получаем номер вызова программы и обновляем его.
            RewriteOutput(c, p, generation); // Записываем данные в output.txt
            Console.WriteLine("Для выхода нажмите Escape....", "\n"); // Ждём выхода из программы
            keyToExit = Console.ReadKey();
            Console.Clear(); // Очищаем консоль от лишнего "задвоения" информации.
        } while (keyToExit.Key != ConsoleKey.Escape); // Окончание цикла решения.
    }
}