using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SharpLab;

public class StudentCollection(string name = "StudentCollection")
{
    private readonly List<Student> _students = new();
    private SortedList<string, Student> _sorted = new();
    private ImmutableList<Student> _immutable = ImmutableList<Student>.Empty;

    public string CollectionName { get; set; } = name;

    public event StudentListHandler? StudentCountChanged;
    public event StudentListHandler? StudentReferenceChanged;

    private void RaiseCountChanged(string info, Student? student) =>
        StudentCountChanged?.Invoke(
            this, new StudentListHandlerEventArgs(CollectionName, info, student));

    private void RaiseRefChanged(string info, Student? student) =>
        StudentReferenceChanged?.Invoke(
            this, new StudentListHandlerEventArgs(CollectionName, info, student));

    public Student this[int index]
    {
        get => _students[index];
        set
        {
            if (index < 0 || index >= _students.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            var old = _students[index];
            _sorted.Remove(MakeSortKey(old));
            _students[index] = value;
            _sorted[MakeSortKey(value)] = value;
            _immutable = _students.ToImmutableList();
            RaiseRefChanged($"Замінено елемент [{index}]", value);
        }
    }

    private static string MakeSortKey(Student s) =>
        $"{s.Person.LastName}|{s.Person.FirstName}|{s.GroupNumber:D4}";

    public void AddStudents(params Student[] students)
    {
        foreach (var s in students)
        {
            _students.Add(s);
            _sorted[MakeSortKey(s)] = s;
            RaiseCountChanged("Додано студента", s);
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

    public bool Remove(int j)
    {
        if (j < 0 || j >= _students.Count) return false;
        var removed = _students[j];
        _sorted.Remove(MakeSortKey(removed));
        _students.RemoveAt(j);
        _immutable = _students.ToImmutableList();
        RaiseCountChanged("Видалено студента", removed);
        return true;
    }

    public int Count => _students.Count;

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

    public override string ToString() =>
        BuildFull($"{CollectionName} / List<Student>", _students);

    public string ToSortedString() =>
        BuildFull($"{CollectionName} / SortedList (alphabetical)", _sorted.Values);

    public string ToImmutableString() =>
        BuildFull($"{CollectionName} / ImmutableList (snapshot)", _immutable);

    public string ToShortString() =>
        BuildShort($"{CollectionName} / List<Student>", _students);

    public string ToShortStringSorted() =>
        BuildShort($"{CollectionName} / SortedList (alphabetical)", _sorted.Values);

    public string ToShortStringImmutable() =>
        BuildShort($"{CollectionName} / ImmutableList (snapshot)", _immutable);

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