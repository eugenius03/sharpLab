namespace SharpLab;

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("LAB 6 – Serialization");

        var st1 = new Student(new Person("Ivan", "Lerk", new DateTime(2000, 1, 1)), Education.Bachelor, 311);
        st1.AddExams(new Exam("Math", 5, new DateTime(2023, 1, 1)),
            new Exam("Physics", 4, new DateTime(2023, 2, 1)));
        st1.AddTests(new Test("Programming", true));

        var st2 = (Student)st1.DeepCopy();
        st2.Education = Education.Master;
        st2.AddExams(new Exam("Chemistry", 3, new DateTime(2023, 3, 1)));

        Console.WriteLine("\nOriginal student:");
        Console.WriteLine(st1);
        Console.WriteLine("\nDeep copy (modified):");
        Console.WriteLine(st2);

        Console.Write("\nEnter filename to save/load (e.g. StudentData.json): ");
        var filename = Console.ReadLine() ?? "StudentData.json";

        if (File.Exists("../../../SerializedObjects/" + filename))
        {
            st2.Load(filename);
            Console.WriteLine("\nLoaded from file:");
        }
        else
        {
            st2.Save(filename);
            Console.WriteLine($"\nSaved to {filename}. Loaded student:");
        }
        Console.WriteLine(st2);

        Console.WriteLine("\nAdd exam from console:");
        if (!st2.AddFromConsole())
            Console.WriteLine("Exam not added (invalid input).");

        st2.Save(filename);
        Console.WriteLine("\nUpdated student (after AddFromConsole + Save):");
        Console.WriteLine(st2);

        Student.Load(filename, st2);
        Console.WriteLine("\nReloaded via static Load:");
        Console.WriteLine(st2);

        Console.WriteLine("\nAdd another exam from console:");
        if (!st2.AddFromConsole())
            Console.WriteLine("Exam not added (invalid input).");

        Student.Save(filename, st2);
        Console.WriteLine("\nFinal student (after static Save):");
        Console.WriteLine(st2);

        Console.WriteLine("\nDone. Press any key to exit.");
        Console.ReadKey();
    }
}
