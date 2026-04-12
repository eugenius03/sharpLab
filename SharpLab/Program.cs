using System;

namespace SharpLab;

internal static class Program
{
    private static void Main()
    {
        var col1 = new StudentCollection("Факультет-ІТ");
        var col2 = new StudentCollection("Факультет-CS");

        var journal1 = new Journal();
        var journal2 = new Journal();

        col1.StudentCountChanged     += journal1.OnStudentCountChanged;
        col1.StudentReferenceChanged += journal1.OnStudentReferenceChanged;

        col1.StudentCountChanged     += journal2.OnStudentCountChanged;
        col1.StudentReferenceChanged += journal2.OnStudentReferenceChanged;
        col2.StudentCountChanged     += journal2.OnStudentCountChanged;
        col2.StudentReferenceChanged += journal2.OnStudentReferenceChanged;

        col1.AddDefaults();
        col2.AddDefaults();

        var newStudent1 = new Student(
            new Person("Sofiya", "Marchenko", new DateTime(2003, 4, 18)),
            Education.Bachelor, 302);
        newStudent1.AddExams(new Exam("Algorithms", 5, new DateTime(2026, 1, 20)));
        newStudent1.AddTests(new Test("Algorithms", true));

        var newStudent2 = new Student(
            new Person("Dmytro", "Savchuk", new DateTime(2001, 11, 30)),
            Education.SecondEducation, 502);
        newStudent2.AddExams(new Exam("Networks", 4, new DateTime(2026, 2, 5)));

        col1.AddStudents(newStudent1);
        col2.AddStudents(newStudent1, newStudent2);

        col1.Remove(1);
        col2.Remove(0);

        var replacement1 = new Student(
            new Person("Oleksii", "Petorchuk", new DateTime(2002, 8, 7)),
            Education.Master, 202);
        replacement1.AddExams(new Exam("OOP", 5, new DateTime(2026, 1, 10)));
        col1[0] = replacement1;

        int lastIdx = col2.Count - 1;
        var replacement2 = new Student(
            new Person("Yulia", "Bondar", new DateTime(2001, 3, 25)),
            Education.Bachelor, 303);
        replacement2.AddExams(new Exam("Math", 4, new DateTime(2026, 2, 12)));
        col2[lastIdx] = replacement2;

        if (col1.Count > 0)
            col1[0].GroupNumber = 203;

        Console.WriteLine("=== Журнал 1 (Факультет-ІТ) ===");
        Console.WriteLine(journal1);

        Console.WriteLine("\n=== Журнал 2 (Факультет-ІТ + Факультет-CS) ===");
        Console.WriteLine(journal2);

        Console.ReadKey();
    }
}