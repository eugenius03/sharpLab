namespace SharpLab;

internal static class Program
{
    private static void Main()
    {
        var p1 = new Person("Josip", "Klopotenko", new DateTime(2000, 1, 1));
        var p2 = new Person("Josip", "Klopotenko", new DateTime(2000, 1, 1));

        Console.WriteLine("=== Person equality and hash codes ===");
        Console.WriteLine($"ReferenceEquals(p1, p2): {ReferenceEquals(p1, p2)}");
        Console.WriteLine($"p1 == p2: {p1 == p2}");
        Console.WriteLine($"p1.Equals(p2): {p1.Equals(p2)}");
        Console.WriteLine($"p1.GetHashCode(): {p1.GetHashCode()}");
        Console.WriteLine($"p2.GetHashCode(): {p2.GetHashCode()}");
        Console.WriteLine();

        var st = new Student(
            new Person("Oleg", "Mimchinckii", new DateTime(2003, 5, 12)),
            Education.Master,
            310);

        st.AddExams(
            new Exam("OOP", 5, new DateTime(2025, 12, 20)),
            new Exam("Databases", 4, new DateTime(2025, 12, 22)),
            new Exam("Networks", 3, new DateTime(2026, 1, 10)),
            new Exam("Algorithms", 5, new DateTime(2026, 1, 15))
        );

        st.AddTests(
            new Test("OOP", true),
            new Test("Databases", true),
            new Test("Networks", false),
            new Test("Philosophy", true)
        );

        Console.WriteLine("=== Student full data ===");
        Console.WriteLine(st.ToString());
        Console.WriteLine();

        Console.WriteLine("=== Student.Person property ===");
        Console.WriteLine(st.Person);
        Console.WriteLine();

        var stCopy = (Student)st.DeepCopy();

        st.Person = new Person("Changed", "Name", new DateTime(1999, 9, 9));
        st.GroupNumber = 350;
        st.AddExams(new Exam("Linear Algebra", 4, new DateTime(2026, 2, 1)));
        st.AddTests(new Test("Linear Algebra", true));

        Console.WriteLine("=== Original student after changes ===");
        Console.WriteLine(st.ToString());
        Console.WriteLine();

        Console.WriteLine("=== Deep copy of student (must be unchanged) ===");
        Console.WriteLine(stCopy.ToString());
        Console.WriteLine();

        Console.WriteLine("=== GroupNumber validation (exception demo) ===");
        try
        {
            st.GroupNumber = 50; // invalid
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine("Error setting GroupNumber: " + ex.Message);
        }

        Console.WriteLine();

        Console.WriteLine("=== Exams with grade > 3 for original student ===");
        foreach (Exam e in st.ExamsWithGradeGreaterThan(3)) Console.WriteLine(e);
        Console.WriteLine();


        Console.WriteLine("=== Subjects that are in BOTH tests and exams ===");
        foreach (string subject in st) Console.WriteLine(subject);
        Console.WriteLine();

        Console.WriteLine("=== All passed tests and exams ===");
        foreach (var obj in st.PassedResults()) Console.WriteLine(obj);
        Console.WriteLine();

        Console.WriteLine("=== Passed tests with passed exams ===");
        foreach (Test t in st.PassedTestsWithPassedExam()) Console.WriteLine(t);
        Console.WriteLine();
    }
}