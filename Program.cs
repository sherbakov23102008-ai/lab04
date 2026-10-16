using System;
using System.Threading;
using System.Threading.Tasks;

Console.WriteLine("Информация о процессоре");
Console.WriteLine($"Доступно логических процессоров: {Environment.ProcessorCount}");
Console.Write("Введите количество потоков (1, 2, 4, 8, 16): ");

if (!int.TryParse(Console.ReadLine(), out int threadCount) || threadCount < 1)
{
    Console.WriteLine("Введите положительное целое число.");
    return;
}

threadCount = Math.Min(threadCount, Environment.ProcessorCount * 2);
Console.WriteLine($"Запускаем потоков: {threadCount}");
Console.WriteLine("Откройте Диспетчер задач → Производительность → ЦП.");

using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
Task[] tasks = new Task[threadCount];

for (int i = 0; i < threadCount; i++)
{
    tasks[i] = Task.Run(() =>
    {
        double value = 0;
        while (!cts.Token.IsCancellationRequested)
        {
            value += Math.Sqrt(12345.6789);
            if (value > 1_000_000) value = 0;
        }
    });
}

await Task.WhenAll(tasks);
Console.WriteLine("Эксперимент завершён.");
