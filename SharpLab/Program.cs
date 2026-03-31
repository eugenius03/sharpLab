using System;
using System.Linq;

namespace SharpLab;

internal static class Program
{
    private static void Main()
    {
        // =====================================================================
        // LAB 3  PART 1 – Build StudentCollection
        // =====================================================================
        Console.WriteLine("=== LAB 3  PART 1 – StudentCollection ===\n");

        var collection = new StudentCollection();

        var s1 = new Student(new Person("Anna",   "Maria", new DateTime(2001, 3, 15)), Education.Master,          201);
        s1.AddExams(new Exam("OOP",       5, new DateTime(2025, 12, 20)),
                    new Exam("Databases", 4, new DateTime(2025, 12, 22)));
        s1.AddTests(new Test("OOP", true), new Test("Databases", true));

        var s2 = new Student(new Person("Andrii", "Aniki", new DateTime(2002, 7, 22)), Education.Bachelor,        301);
        s2.AddExams(new Exam("Networks", 3, new DateTime(2026, 1, 10)),
                    new Exam("Math",     2, new DateTime(2026, 1, 12)));
        s2.AddTests(new Test("Networks", false));

        var s3 = new Student(new Person("Semen",  "Brat",  new DateTime(2000, 11, 5)), Education.Master,          401);
        s3.AddExams(new Exam("Algorithms", 5, new DateTime(2026, 1, 15)),
                    new Exam("OOP",        5, new DateTime(2025, 12, 20)));
        s3.AddTests(new Test("Algorithms", true), new Test("OOP", true));

        var s4 = new Student(new Person("Stepan", "Farm",  new DateTime(2003, 6, 1)), Education.SecondEducation, 501);
        s4.AddExams(new Exam("Physics", 4, new DateTime(2026, 2, 1)));

        collection.AddStudents(s1, s2, s3, s4);

        Console.WriteLine(collection.ToString());
        
        Console.WriteLine("=== LAB 3  PART 2 – Sorting ===\n");

        Console.WriteLine("-- Sorted by last name (IComparable) --");
        collection.SortByLastName();
        Console.WriteLine(collection.ToShortString());

        Console.WriteLine("-- Sorted by birth date (IComparer<Person>) --");
        collection.SortByBirthDate();
        Console.WriteLine(collection.ToShortString());

        Console.WriteLine("-- Sorted by average grade (IComparer<Student>) --");
        collection.SortByAverageGrade();
        Console.WriteLine(collection.ToShortString());
        
        Console.WriteLine("-- SortedList view (always alphabetical by name, independent of above) --");
        Console.WriteLine(collection.ToShortStringSorted());
        
        Console.WriteLine("-- ImmutableList snapshot (mirrors last sort applied to List) --");
        Console.WriteLine(collection.ToShortStringImmutable());
        
        Console.WriteLine("=== LAB 3  PART 3 – LINQ queries (List vs SortedList vs ImmutableList) ===\n");
        
        Console.WriteLine("MaxAverageGrade:");
        Console.WriteLine($"  List<Student>            : {collection.MaxAverageGrade:F2}");
        Console.WriteLine($"  SortedList               : {collection.MaxAverageGradeSorted:F2}");
        Console.WriteLine($"  ImmutableList            : {collection.MaxAverageGradeImmutable:F2}");
        
        Console.WriteLine("\nMaster students (Education.Master):");

        Console.WriteLine("  [List]");
        foreach (var s in collection.MasterStudents)
            Console.WriteLine("    " + s.ToShortString());

        Console.WriteLine("  [SortedList]");
        foreach (var s in collection.MasterStudentsSorted)
            Console.WriteLine("    " + s.ToShortString());

        Console.WriteLine("  [ImmutableList]");
        foreach (var s in collection.MasterStudentsImmutable)
            Console.WriteLine("    " + s.ToShortString());
        
        Console.WriteLine("\nGroups by rounded average grade:");
        var uniqueAvgs = new[] { s1, s2, s3, s4 }
            .Select(s => Math.Round(s.AverageGrade, 2))
            .Distinct()
            .OrderBy(x => x);

        foreach (var avg in uniqueAvgs)
        {
            Console.WriteLine($"\n  avg = {avg:F2}");

            var fromList      = collection.AverageMarkGroup(avg);
            var fromSorted    = collection.AverageMarkGroupSorted(avg);
            var fromImmutable = collection.AverageMarkGroupImmutable(avg);

            PrintGroup("List",          fromList);
            PrintGroup("SortedList",    fromSorted);
            PrintGroup("ImmutableList", fromImmutable);
        }
        
        Console.WriteLine("\n=== LAB 4 – TestCollections benchmark ===\n");
        Console.WriteLine("Standard vs Immutable vs Sorted (ContainsKey / ContainsValue timing)\n");

        int size = 0;
        while (true)
        {
            Console.Write("Enter the number of elements for TestCollections: ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out var v) && v > 0) { size = v; break; }
            Console.WriteLine("  Please enter a positive integer.");
        }

        var tc = new TestCollections(size);
        tc.MeasureSearchTimes();

        Console.WriteLine("\nDone. Press any key to exit.");
        Console.ReadKey();
    }

    private static void PrintGroup(string label, System.Collections.Generic.List<Student> group)
    {
        if (group.Count == 0)
            Console.WriteLine($"    [{label}] (none)");
        else
            foreach (var s in group)
                Console.WriteLine($"    [{label}] {s.ToShortString()}");
    }
}
