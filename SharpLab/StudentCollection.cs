using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SharpLab;

public class StudentCollection
{
    private readonly List<Student> _students = new();
    private SortedList<string, Student> _sorted = new();
    private ImmutableList<Student> _immutable = ImmutableList<Student>.Empty;
    
    private static string MakeSortKey(Student s) =>
        $"{s.Person.LastName}|{s.Person.FirstName}|{s.GroupNumber:D4}";
    
    public void AddStudents(params Student[] students)
    {
        foreach (var s in students)
        {
            _students.Add(s);
            _sorted[MakeSortKey(s)] = s;
        }
        _immutable = _students.ToImmutableList();
    }

    public void AddDefaults()
    {
        AddStudents(
            new Student(new Person("Ihor", "Karpenko", new DateTime(2001, 3, 15)), Education.Master, 201),
            new Student(new Person("Ivan", "Huchko",   new DateTime(2002, 7, 22)), Education.Bachelor, 301),
            new Student(new Person("Leon", "Rider",    new DateTime(2000, 11, 5)), Education.Master, 401)
        );
    }

    public void SortByLastName()
    {
        _students.Sort();
        _immutable = _students.ToImmutableList();
    }

    public void SortByBirthDate()
    {
        _students.Sort(new Person());
        _immutable = _students.ToImmutableList();
    }

    public void SortByAverageGrade()
    {
        _students.Sort(new StudentAverageGradeComparer());
        _immutable = _students.ToImmutableList();
    }

    // -------------------------------------------------------------------------
    // Queries — List<Student>
    // -------------------------------------------------------------------------

    public double MaxAverageGrade =>
        _students.Count == 0 ? 0.0 : _students.Max(s => s.AverageGrade);

    public IEnumerable<Student> MasterStudents =>
        _students.Where(s => s.Education == Education.Master);

    public List<Student> AverageMarkGroup(double targetAvg)
    {
        var rounded = Math.Round(targetAvg, 2);
        return _students
            .GroupBy(s => Math.Round(s.AverageGrade, 2))
            .FirstOrDefault(g => g.Key == rounded)
            ?.ToList() ?? new List<Student>();
    }

    public double MaxAverageGradeSorted =>
        _sorted.Count == 0 ? 0.0 : _sorted.Values.Max(s => s.AverageGrade);

    public IEnumerable<Student> MasterStudentsSorted =>
        _sorted.Values.Where(s => s.Education == Education.Master);

    public List<Student> AverageMarkGroupSorted(double targetAvg)
    {
        var rounded = Math.Round(targetAvg, 2);
        return _sorted.Values
            .GroupBy(s => Math.Round(s.AverageGrade, 2))
            .FirstOrDefault(g => g.Key == rounded)
            ?.ToList() ?? new List<Student>();
    }

    // -------------------------------------------------------------------------
    // Queries — ImmutableList<Student>
    // -------------------------------------------------------------------------

    public double MaxAverageGradeImmutable =>
        _immutable.Count == 0 ? 0.0 : _immutable.Max(s => s.AverageGrade);

    public IEnumerable<Student> MasterStudentsImmutable =>
        _immutable.Where(s => s.Education == Education.Master);

    public List<Student> AverageMarkGroupImmutable(double targetAvg)
    {
        var rounded = Math.Round(targetAvg, 2);
        return _immutable
            .GroupBy(s => Math.Round(s.AverageGrade, 2))
            .FirstOrDefault(g => g.Key == rounded)
            ?.ToList() ?? new List<Student>();
    }

    // -------------------------------------------------------------------------
    // String representations for all three
    // -------------------------------------------------------------------------

    public override string ToString() =>
        BuildFull("List<Student>", _students);

    public string ToSortedString() =>
        BuildFull("SortedList (alphabetical by name)", _sorted.Values);

    public string ToImmutableString() =>
        BuildFull("ImmutableList<Student> (current snapshot)", _immutable);

    public string ToShortString() =>
        BuildShort("List<Student>", _students);

    public string ToShortStringSorted() =>
        BuildShort("SortedList (alphabetical by name)", _sorted.Values);

    public string ToShortStringImmutable() =>
        BuildShort("ImmutableList<Student> (current snapshot)", _immutable);

    private static string BuildFull(string title, IEnumerable<Student> source)
    {
        var items = source.ToList();
        if (items.Count == 0) return $"({title}: empty)\n";
        var sb = new StringBuilder();
        sb.AppendLine($"{title}  [{items.Count} student(s)]:");
        foreach (var s in items)
        {
            sb.AppendLine(new string('-', 60));
            sb.AppendLine(s.ToString());
        }
        sb.AppendLine(new string('-', 60));
        return sb.ToString();
    }

    private static string BuildShort(string title, IEnumerable<Student> source)
    {
        var items = source.ToList();
        if (items.Count == 0) return $"({title}: empty)\n";
        var sb = new StringBuilder();
        sb.AppendLine($"{title}  [{items.Count} student(s)]:");
        foreach (var s in items)
            sb.AppendLine("  • " + s.ToShortString());
        return sb.ToString();
    }
}
