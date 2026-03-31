using System.Text;

namespace SharpLab;

public class StudentCollection
{
    private readonly List<Student> _students = new();


    public double MaxAverageGrade =>
        _students.Count == 0 ? 0.0 : _students.Max(s => s.AverageGrade);

    public IEnumerable<Student> MasterStudents =>
        _students.Where(s => s.Education == Education.Master);

    public void AddDefaults()
    {
        AddStudents(
            new Student(new Person("Ihor", "Karpenko", new DateTime(2001, 3, 15)), Education.Master, 201),
            new Student(new Person("Ivan", "Huchko", new DateTime(2002, 7, 22)), Education.Bachelor, 301),
            new Student(new Person("Leon", "Rider", new DateTime(2000, 11, 5)), Education.Master, 401)
        );
    }

    public void AddStudents(params Student[] students)
    {
        foreach (var s in students)
            _students.Add(s);
    }

    public void SortByLastName()
    {
        _students.Sort();
    }

    public void SortByBirthDate()
    {
        _students.Sort(new Person());
    }

    public void SortByAverageGrade()
    {
        _students.Sort(new StudentAverageGradeComparer());
    }

    public List<Student> AverageMarkGroup(double value)
    {
        var rounded = Math.Round(value, 2);
        return _students
            .GroupBy(s => Math.Round(s.AverageGrade, 2))
            .FirstOrDefault(g => g.Key == rounded)
            ?.ToList() ?? new List<Student>();
    }


    public override string ToString()
    {
        if (_students.Count == 0) return "(empty collection)";
        var sb = new StringBuilder();
        sb.AppendLine($"StudentCollection ({_students.Count} students):");
        foreach (var s in _students)
        {
            sb.AppendLine(new string('-', 60));
            sb.AppendLine(s.ToString());
        }

        sb.AppendLine(new string('-', 60));
        return sb.ToString();
    }

    public string ToShortString()
    {
        if (_students.Count == 0) return "(empty collection)";
        var sb = new StringBuilder();
        sb.AppendLine($"StudentCollection ({_students.Count} students):");
        foreach (var s in _students)
            sb.AppendLine(" • " + s.ToShortString());
        return sb.ToString();
    }
}