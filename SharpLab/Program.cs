namespace SharpLab;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine(" LAB 3  PART 1 – StudentCollection");

        var collection = new StudentCollection();

        var s1 = new Student(new Person("Anna", "Kovalenko", new DateTime(2001, 3, 15)), Education.Master, 201);
        s1.AddExams(new Exam("OOP", 5, new DateTime(2025, 12, 20)),
            new Exam("Databases", 4, new DateTime(2025, 12, 22)));
        s1.AddTests(new Test("OOP", true), new Test("Databases", true));

        var s2 = new Student(new Person("Ivan", "Petrenko", new DateTime(2002, 7, 22)), Education.Bachelor, 301);
        s2.AddExams(new Exam("Networks", 3, new DateTime(2026, 1, 10)),
            new Exam("Math", 2, new DateTime(2026, 1, 12)));
        s2.AddTests(new Test("Networks", false));

        var s3 = new Student(new Person("Maria", "Shevchenko", new DateTime(2000, 11, 5)), Education.Master, 401);
        s3.AddExams(new Exam("Algorithms", 5, new DateTime(2026, 1, 15)),
            new Exam("OOP", 5, new DateTime(2025, 12, 20)));
        s3.AddTests(new Test("Algorithms", true), new Test("OOP", true));

        var s4 = new Student(new Person("Olha", "Bondarenko", new DateTime(2003, 6, 1)), Education.SecondEducation,
            501);
        s4.AddExams(new Exam("Physics", 4, new DateTime(2026, 2, 1)));

        collection.AddStudents(s1, s2, s3, s4);

        Console.WriteLine("\n--- Full StudentCollection (ToString) ---");
        Console.WriteLine(collection);

        Console.WriteLine(" LAB 3  PART 2 – Sorting");

        Console.WriteLine("\n--- Sorted by Last Name (IComparable) ---");
        collection.SortByLastName();
        Console.WriteLine(collection.ToShortString());

        Console.WriteLine("--- Sorted by Birth Date (IComparer<Person>) ---");
        collection.SortByBirthDate();
        Console.WriteLine(collection.ToShortString());

        Console.WriteLine("--- Sorted by Average Grade (IComparer<Student>) ---");
        collection.SortByAverageGrade();
        Console.WriteLine(collection.ToShortString());

        Console.WriteLine(" LAB 3  PART 3 – LINQ Operations");

        Console.WriteLine($"\nMax average grade: {collection.MaxAverageGrade:F2}");

        Console.WriteLine("\nMaster students (Education.Master):");
        foreach (var s in collection.MasterStudents)
            Console.WriteLine("  " + s.ToShortString());

        Console.WriteLine("\nGroups by average grade:");
        var uniqueAvgs = new[] { s1, s2, s3, s4 }
            .Select(s => Math.Round(s.AverageGrade, 2))
            .Distinct()
            .OrderBy(x => x);

        foreach (var avg in uniqueAvgs)
        {
            var group = collection.AverageMarkGroup(avg);
            Console.WriteLine($"  Avg = {avg:F2}  ({group.Count} student(s)):");
            foreach (var s in group)
                Console.WriteLine("    " + s.ToShortString());
        }

        Console.WriteLine(" LAB 3  PART 4 – TestCollections (search timing)");

        var size = 0;
        while (true)
        {
            Console.Write("Enter the number of elements for TestCollections: ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out var value) && value > 0)
            {
                size = value;
                break;
            }

            Console.WriteLine("  Error: please enter a positive integer.");
        }

        var tc = new TestCollections(size);
        tc.MeasureSearchTimes();

        Console.WriteLine("\nDone. Press any key to exit.");
        Console.ReadKey();
    }
}