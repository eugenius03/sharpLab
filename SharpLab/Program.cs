using System.Threading.Tasks.Dataflow;

namespace SharpLab;

class Program
{
    static async Task Main()
    {
        var rng = new Random(42);
        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
            Console.WriteLine("\nStop requested...");
        };

        Console.WriteLine("=== Matrix Multiplication via TPL Dataflow ===");
        Console.WriteLine("Ctrl+C to cancel at any time.\n");

        while (!cts.Token.IsCancellationRequested)
        {
            Console.Write("Enter rows A: ");
            if (!int.TryParse(Console.ReadLine(), out int ra) || ra <= 0) break;
            Console.Write("Enter cols A (= rows B): ");
            if (!int.TryParse(Console.ReadLine(), out int ca) || ca <= 0) break;
            Console.Write("Enter cols B: ");
            if (!int.TryParse(Console.ReadLine(), out int cb) || cb <= 0) break;

            Console.WriteLine("\nGenerating matrices...");
            var a = GenerateMatrix(ra, ca, rng);
            var b = GenerateMatrix(ca, cb, rng);

            if (ra <= 10 && ca <= 10) PrintMatrix(a, "A");
            if (ca <= 10 && cb <= 10) PrintMatrix(b, "B");

            Console.WriteLine($"Multiplying [{ra}x{ca}] * [{ca}x{cb}] = [{ra}x{cb}]...");
            var sw = System.Diagnostics.Stopwatch.StartNew();

            var result = await MultiplyDataflow(a, b, cts.Token);
            sw.Stop();

            if (result != null)
            {
                Console.WriteLine($"Done in {sw.Elapsed.TotalSeconds:F2}s\n");
                if (ra <= 10 && cb <= 10) PrintMatrix(result, "Result");
            }

            if (cts.Token.IsCancellationRequested) break;

            Console.Write("\nMultiply again? (y/n): ");
            var ans = Console.ReadLine();
            if (ans?.Trim().ToLower() != "y") break;
        }

        Console.WriteLine("Bye.");
    }

    static double[,] GenerateMatrix(int rows, int cols, Random rng)
    {
        var m = new double[rows, cols];
        for (int i = 0; i < rows; i++)
        for (int j = 0; j < cols; j++)
            m[i, j] = Math.Round(rng.NextDouble() * 10 - 5, 1);
        return m;
    }

    static void PrintMatrix(double[,] m, string name)
    {
        int rows = m.GetLength(0), cols = m.GetLength(1);
        Console.WriteLine($"{name} [{rows}x{cols}]:");
        int maxRows = Math.Min(rows, 6), maxCols = Math.Min(cols, 6);
        for (int i = 0; i < maxRows; i++)
        {
            for (int j = 0; j < maxCols; j++)
                Console.Write($"{m[i, j],7:F1}");
            if (cols > maxCols) Console.Write(" ...");
            Console.WriteLine();
        }
        if (rows > maxRows) Console.WriteLine("  ...");
        Console.WriteLine();
    }

    static async Task<double[,]?> MultiplyDataflow(double[,] a, double[,] b, CancellationToken ct)
    {
        int ra = a.GetLength(0), ca = a.GetLength(1);
        int rb = b.GetLength(0), cb = b.GetLength(1);

        if (ca != rb)
        {
            Console.WriteLine($"Cannot multiply: A columns ({ca}) != B rows ({rb})");
            return null;
        }

        var result = new double[ra, cb];
        long total = (long)ra * cb;
        long done = 0;

        int maxDop = Math.Max(1, Environment.ProcessorCount - 1);

        var computeBlock = new ActionBlock<(int row, int col)>(
            pos =>
            {
                if (ct.IsCancellationRequested) return;
                double sum = 0;
                for (int k = 0; k < ca; k++)
                    sum += a[pos.row, k] * b[k, pos.col];
                result[pos.row, pos.col] = sum;

                long d = Interlocked.Increment(ref done);
                if (total <= 100 || d % (total / 100) == 0)
                {
                    int pct = (int)(d * 100 / total);
                    Console.Write($"\rProgress: {pct,3}%  ({d}/{total})");
                }
            },
            new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDop,
                CancellationToken = ct,
                BoundedCapacity = maxDop * 4
            });

        for (int i = 0; i < ra && !ct.IsCancellationRequested; i++)
        for (int j = 0; j < cb && !ct.IsCancellationRequested; j++)
            await computeBlock.SendAsync((i, j), ct);

        computeBlock.Complete();
        try { await computeBlock.Completion; }
        catch (OperationCanceledException) { Console.WriteLine("\nCancelled."); return null; }

        Console.WriteLine($"\rProgress: 100%  ({total}/{total})");
        return result;
    }
}