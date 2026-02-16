namespace SharpLab;

internal static class Program
{
    private static void Main()
    {
        var st = new Student();
        Console.WriteLine("=== ToShortString() for default student ===");
        Console.WriteLine(st.ToShortString());
        Console.WriteLine();

        Console.WriteLine("=== Indexer results ===");
        Console.WriteLine($"Master: {st[Education.Master]}");
        Console.WriteLine($"Bachelor: {st[Education.Bachelor]}");
        Console.WriteLine($"SecondEducation: {st[Education.SecondEducation]}");
        Console.WriteLine();

        st = new Student
        {
            Person = new Person("Yevhenii", "Popov", new DateTime(2003, 5, 12)),
            Education = Education.Master,
            GroupNumber = 310,
            Exams = new[]
            {
                new Exam("OOP", 95, new DateTime(2025, 12, 20)),
                new Exam("Databases", 90, new DateTime(2025, 12, 22))
            }
        };

        Console.WriteLine("=== ToString() after assigning properties ===");
        Console.WriteLine(st.ToString());
        Console.WriteLine();

        st.AddExams(
            new Exam("Networks", 88, new DateTime(2026, 1, 10)),
            new Exam("Algorithms", 92, new DateTime(2026, 1, 15))
        );

        Console.WriteLine("=== ToString() after AddExams(...) ===");
        Console.WriteLine(st.ToString());
        Console.WriteLine();

        Console.WriteLine("=== Array performance comparison (Exam) ===");
        Console.WriteLine("Enter nRows and nColumns in one line.");
        Console.WriteLine("Separators: space, comma, semicolon, tab. Example: 500, 500");
        Console.Write("> ");

        var line = Console.ReadLine() ?? "";
        var parts = line.Split([' ', ',', ';', '\t'], StringSplitOptions.RemoveEmptyEntries);

        var nRows = int.Parse(parts[0]);
        var nCols = int.Parse(parts[1]);

        var total = checked(nRows * nCols);

        var oneDim = new Exam[total];
        for (var i = 0; i < oneDim.Length; i++)
            oneDim[i] = new Exam();

        var rect = new Exam[nRows, nCols];
        for (var r = 0; r < nRows; r++)
        for (var c = 0; c < nCols; c++)
            rect[r, c] = new Exam();

        var jagged = new Exam[nRows][];
        for (var r = 0; r < nRows; r++)
        {
            jagged[r] = new Exam[nCols];
            for (var c = 0; c < nCols; c++)
                jagged[r][c] = new Exam();
        }

        var t1 = MeasureOneDim(oneDim);
        var t2 = MeasureRect(rect, nRows, nCols);
        var t3 = MeasureJagged(jagged, nRows, nCols);

        Console.WriteLine();
        Console.WriteLine($"nRows={nRows}, nColumns={nCols}, totalElements={total}");
        Console.WriteLine($"1D Exam[] time:            {t1} ms");
        Console.WriteLine($"2D Exam[,] time:           {t2} ms");
        Console.WriteLine($"Jagged Exam[][] time:      {t3} ms");
    }

    private static int MeasureOneDim(Exam[] arr)
    {
        var start = Environment.TickCount;
        for (var i = 0; i < arr.Length; i++)
            arr[i].Grade = 100;
        var end = Environment.TickCount;
        return end - start;
    }

    private static int MeasureRect(Exam[,] arr, int nRows, int nCols)
    {
        var start = Environment.TickCount;
        for (var r = 0; r < nRows; r++)
        for (var c = 0; c < nCols; c++)
            arr[r, c].Grade = 100;
        var end = Environment.TickCount;
        return end - start;
    }

    private static int MeasureJagged(Exam[][] arr, int nRows, int nCols)
    {
        var start = Environment.TickCount;
        for (var r = 0; r < nRows; r++)
        for (var c = 0; c < nCols; c++)
            arr[r][c].Grade = 100;
        var end = Environment.TickCount;
        return end - start;
    }
}