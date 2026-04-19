namespace SharpLab;
public static class ConsoleHelper
{

    public static void Write(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    public static void WriteLine(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
    
    public static void PrintType(IHasName obj)
    {
        Write($"[{obj.GetType().Name}]", ConsoleColor.Yellow);
    }

    public static void PrintName(IHasName obj)
    {
        Write(obj.Name, ConsoleColor.Cyan);
    }


    public static void PrintHeader()
    {
        Console.Clear();
        WriteLine("╔══════════════════════════════════════╗", ConsoleColor.DarkCyan);
        WriteLine("║      СИМУЛЯТОР ЗУСТРІЧЕЙ  v1.0       ║", ConsoleColor.Cyan);
        WriteLine("╚══════════════════════════════════════╝", ConsoleColor.DarkCyan);
        Console.WriteLine();
    }

    public static void PrintInstructions()
    {
        WriteLine("  Enter  - наступні дві пари", ConsoleColor.DarkGray);
        WriteLine("  T      - тестовий режим (вибір пари вручну)", ConsoleColor.DarkGray);
        WriteLine("  Q / q  - вихід    F10 - вихід", ConsoleColor.DarkGray);
        Console.WriteLine();
    }

    public static void PrintPairResult(Human first, Human second, IHasName? child,
        bool firstLikes, bool secondLikes)
    {
        Write("  ── ", ConsoleColor.DarkGray);
        Write(first.GetType().Name,  ConsoleColor.White);
        Write($" ({first.Name})", ConsoleColor.Gray);
        Write("  +  ", ConsoleColor.DarkGray);
        Write(second.GetType().Name, ConsoleColor.White);
        WriteLine($" ({second.Name})", ConsoleColor.Gray);

        string like1 = firstLikes  ? "подобається ✓" : "не подобається ✗";
        string like2 = secondLikes ? "подобається ✓" : "не подобається ✗";
        Write   ($"     {first.GetType().Name,-12}: ", ConsoleColor.Gray);
        WriteLine(like1, firstLikes  ? ConsoleColor.Green : ConsoleColor.Red);
        Write   ($"     {second.GetType().Name,-12}: ", ConsoleColor.Gray);
        WriteLine(like2, secondLikes ? ConsoleColor.Green : ConsoleColor.Red);

        if (child != null)
        {
            Write("     Результат → ", ConsoleColor.DarkGray);
            PrintType(child);
            Write(" ", ConsoleColor.White);
            PrintName(child);

            if (child is Human h)
            {
                var patronymicProp = h.GetType().GetProperty("Patronymic");
                if (patronymicProp != null)
                {
                    string? p = (string?)patronymicProp.GetValue(h);
                    if (!string.IsNullOrEmpty(p))
                        Write($" {p}", ConsoleColor.DarkCyan);
                }
            }
            Console.WriteLine();
        }
        else
        {
            WriteLine("     Пара не склалась.", ConsoleColor.DarkGray);
        }

        Console.WriteLine();
    }

    public static void PrintSameGenderError(SameGenderException ex)
    {
        WriteLine($"     [Однакова стать] {ex.Message}", ConsoleColor.Magenta);
        Console.WriteLine();
    }
}
