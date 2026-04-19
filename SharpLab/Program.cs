namespace SharpLab;

internal static class Program
{
    private static readonly Random Rng = new();

    private static readonly Human[] All =
    [
        new Student    { Name = "Олексій"  },
        new Student    { Name = "Максим"   },
        new Botan      { Name = "Дмитро"   },
        new Botan      { Name = "Ігор"     },
        new Girl       { Name = "Олена"    },
        new Girl       { Name = "Марія"    },
        new PrettyGirl { Name = "Анна"     },
        new PrettyGirl { Name = "Вікторія" },
        new SmartGirl  { Name = "Наталія"  },
        new SmartGirl  { Name = "Юлія"     },
    ];

    private static void Main()
    {
        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
        {
            ConsoleHelper.WriteLine("Консоль не працює по неділях!", ConsoleColor.Red);
            return;
        }

        ConsoleHelper.PrintHeader();
        ConsoleHelper.PrintInstructions();

        while (true)
        {
            ShowPair(PickRandom(), PickRandom());
            ShowPair(PickRandom(), PickRandom());

            ConsoleHelper.WriteLine("  [ Enter - далі | T - тест | Q/F10 - вихід ]",
                ConsoleColor.DarkGray);

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key is ConsoleKey.F10 or ConsoleKey.Q)
                break;

            if (key.Key is ConsoleKey.T)
                RunManualTest();

            Console.WriteLine();
        }

        ConsoleHelper.WriteLine("\nДо побачення!", ConsoleColor.DarkCyan);
    }

    private static Human PickRandom()
    {
        return All[Rng.Next(All.Length)];
    }

    private static void ShowPair(Human first, Human second)
    {
        while (ReferenceEquals(first, second))
            second = PickRandom();

        try
        {
            IHasName? child = CoupleService.Couple(first, second,
                out bool firstLikes, out bool secondLikes);

            ConsoleHelper.PrintPairResult(first, second, child, firstLikes, secondLikes);
        }
        catch (SameGenderException ex)
        {
            ConsoleHelper.Write("  ── ", ConsoleColor.DarkGray);
            ConsoleHelper.Write(first.GetType().Name,  ConsoleColor.White);
            ConsoleHelper.Write($" ({first.Name})", ConsoleColor.Gray);
            ConsoleHelper.Write("  +  ", ConsoleColor.DarkGray);
            ConsoleHelper.Write(second.GetType().Name, ConsoleColor.White);
            ConsoleHelper.WriteLine($" ({second.Name})", ConsoleColor.Gray);
            ConsoleHelper.PrintSameGenderError(ex);
        }
    }

    private static void RunManualTest()
    {
        Console.WriteLine();
        ConsoleHelper.WriteLine("  ═══ ТЕСТОВИЙ РЕЖИМ ═══", ConsoleColor.Cyan);

        for (int i = 0; i < All.Length; i++)
        {
            ConsoleHelper.Write($"  {i,2}: ", ConsoleColor.DarkGray);
            ConsoleHelper.Write($"{All[i].GetType().Name,-12}", ConsoleColor.White);
            ConsoleHelper.WriteLine($" ({All[i].Name})", ConsoleColor.Gray);
        }

        Console.WriteLine();
        ConsoleHelper.Write("  Перший (0-9): ", ConsoleColor.DarkGray);
        int idxA = ReadIndex();

        ConsoleHelper.Write("  Другий (0-9): ", ConsoleColor.DarkGray);
        int idxB = ReadIndex();

        Console.WriteLine();
        if (idxA >= 0 && idxB >= 0 && idxA < All.Length && idxB < All.Length)
            ShowPair(All[idxA], All[idxB]);
        else
            ConsoleHelper.WriteLine("  Невірний індекс.", ConsoleColor.Red);
    }

    private static int ReadIndex()
    {
        string? input = Console.ReadLine();
        return int.TryParse(input?.Trim(), out int idx) ? idx : -1;
    }
}
