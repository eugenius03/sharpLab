using System.Text;

namespace SharpLab;

public class Student(Person person, Education education, int groupNumber)
{
    private Exam[] _exams = Array.Empty<Exam>();

    public Student()
        : this(new Person(), Education.Bachelor, 101)
    {
    }

    public Person Person { get; init; } = person;
    public Education Education { get; init; } = education;
    public int GroupNumber { get; init; } = groupNumber;

    public Exam[] Exams
    {
        get => (Exam[])_exams.Clone();
        init => _exams = value ?? Array.Empty<Exam>();
    }

    private double AverageGrade
        => _exams.Length == 0 ? 0.0 : _exams.Average(e => e.Grade);

    public bool this[Education edu] => Education == edu;

    public void AddExams(params Exam[] exams)
    {
        if (exams.Length == 0) return;

        var oldLen = _exams.Length;
        Array.Resize(ref _exams, oldLen + exams.Length);
        Array.Copy(exams, 0, _exams, oldLen, exams.Length);
    }

    public override string ToString()
    {
        var header =
            $"Student: [{Person}], Education={Education}, GroupNumber={GroupNumber}\n" +
            $"ExamsCount={_exams.Length}";

        if (_exams.Length == 0) return header + "\nExams: (none)";

        var examsText = new StringBuilder("Exams:\n");
        foreach (var t in _exams)
            examsText.Append($"  - {t}\n");

        return header + "\n" + examsText;
    }

    public string ToShortString()
    {
        return
            $"Student: [{Person.ToShortString()}], Education={Education}, GroupNumber={GroupNumber}, AvgGrade={AverageGrade}";
    }
}