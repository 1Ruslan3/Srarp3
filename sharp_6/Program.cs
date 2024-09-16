using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Выводим текущую рабочую директорию
        Console.WriteLine($"Текущая директория: {Directory.GetCurrentDirectory()}");

        // Задаем размеры матриц
        int rowsA = 1000, colsA = 1000;
        int rowsB = 1000, colsB = 1000;

        // Путь к директории для сохранения файлов
        string directory = AppDomain.CurrentDomain.BaseDirectory;
        Console.WriteLine($"Директория для файлов: {directory}");

        try
        {
            // Генерация и запись матриц в файлы
            Console.WriteLine("Генерация матрицы A...");
            await MatrixGenerator.GenerateMatrixAsync(Path.Combine(directory, "C:\\Users\\user\\source\\repos\\CS\\sharp_6\\matrixA.txt"), rowsA, colsA);
            Console.WriteLine("Генерация матрицы B...");
            await MatrixGenerator.GenerateMatrixAsync(Path.Combine(directory, "C:\\Users\\user\\source\\repos\\CS\\sharp_6\\matrixB.txt"), rowsB, colsB);

            // Чтение матриц из файлов
            Console.WriteLine("Чтение матрицы A из файла...");
            double[,] matrixA = await MatrixOperations.ReadMatrixFromFileAsync(Path.Combine(directory, "C:\\Users\\user\\source\\repos\\CS\\sharp_6\\matrixA.txt"), rowsA, colsA);
            Console.WriteLine("Чтение матрицы B из файла...");
            double[,] matrixB = await MatrixOperations.ReadMatrixFromFileAsync(Path.Combine(directory, "C:\\Users\\user\\source\\repos\\CS\\sharp_6\\matrixB.txt"), rowsB, colsB);

            // Измерение времени умножения
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Умножение матриц
            Console.WriteLine("Умножение матриц...");
            double[,] result = await MatrixOperations.MultiplyMatricesAsync(matrixA, matrixB, rowsA, colsA, colsB);

            stopwatch.Stop();
            Console.WriteLine($"Время выполнения умножения: {stopwatch.ElapsedMilliseconds} мс");

            // Запись результата в файл
            Console.WriteLine("Запись результата в файл...");
            await MatrixOperations.WriteMatrixToFileAsync(Path.Combine(directory, "C:\\Users\\user\\source\\repos\\CS\\sharp_6\\result.txt"), result, rowsA, colsB);

            Console.WriteLine("Операция завершена успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}

class MatrixGenerator
{
    public static async Task GenerateMatrixAsync(string fileName, int rows, int cols)
    {
        Random rand = new Random();
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    double value = rand.NextDouble() * 100;
                    await writer.WriteAsync($"{value:F2} ");
                }
                await writer.WriteLineAsync();
            }
        }
    }
}

class MatrixOperations
{
    public static async Task<double[,]> ReadMatrixFromFileAsync(string fileName, int rows, int cols)
    {
        double[,] matrix = new double[rows, cols];

        using (StreamReader reader = new StreamReader(fileName))
        {
            for (int i = 0; i < rows; i++)
            {
                string line = await reader.ReadLineAsync();
                string[] values = line.Split(' ');

                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = double.Parse(values[j]);
                }
            }
        }

        return matrix;
    }

    public static async Task<double[,]> MultiplyMatricesAsync(double[,] matrixA, double[,] matrixB, int rowsA, int colsA, int colsB)
    {
        double[,] result = new double[rowsA, colsB];

        await Task.Run(() =>
        {
            Parallel.For(0, rowsA, i =>
            {
                for (int j = 0; j < colsB; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < colsA; k++)
                    {
                        result[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            });
        });

        return result;
    }

    public static async Task WriteMatrixToFileAsync(string fileName, double[,] matrix, int rows, int cols)
    {                                                                                                     
        using (StreamWriter writer = new StreamWriter(fileName))                                          
        {                                                                                                 
            for (int i = 0; i < rows; i++)                                                                
            {                                                                                             
                for (int j = 0; j < cols; j++)                                                            
                {                                                                                         
                    await writer.WriteAsync($"{matrix[i, j]:F2} ");                                       
                }                                                                                         
                await writer.WriteLineAsync();                                                            
            }                                                                                             
        }
    }
}
