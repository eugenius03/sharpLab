using System.Collections;
using System.Collections.Generic;

namespace SharpLab;

public class Student : Person, IEnumerable
{
    private readonly List<Exam> _exams;
    private readonly List<Test> _tests;
    private int _groupNumber;

    public Student(Person person, Education education, int groupNumber)
        : base(person.FirstName, person.LastName, person.BirthDate)
    {
        Education = education;
        GroupNumber = groupNumber;
        _tests = new List<Test>();
        _exams = new List<Exam>();
    }

    public Student()
        : this(new Person(), Education.Bachelor, 101)
    {
    }

    public Person Person
    {
        get => new(_firstName, _lastName, _birthDate);
        set
        {
            _firstName = value.FirstName;
            _lastName = value.LastName;
            _birthDate = value.BirthDate;
        }
    }

    public Education Education { get; init; }

    public int GroupNumber
    {
        get => _groupNumber;
        set
        {
            if (value <= 100 || value > 699)
                throw new ArgumentOutOfRangeException(
                    "GroupNumber",
                    "GroupNumber must be > 100 and <= 699.");
            _groupNumber = value;
        }
    }

    public List<Exam> Exams
    {
        get => _exams;
        init => _exams = value ?? new List<Exam>();
    }

    public List<Test> Tests
    {
        get => _tests;
        init => _tests = value ?? new List<Test>();
    }

    public double AverageGrade
    {
        get
        {
            if (_exams.Count == 0) return 0.0;
            long sum = 0;
            foreach (var e in _exams)
                if (e != null)
                    sum += e.Grade;
            return (double)sum / _exams.Count;
        }
    }

    public bool this[Education edu] => Education == edu;

    public IEnumerator GetEnumerator()
    {
        return new StudentEnumerator(this);
    }

    public override object DeepCopy()
    {
        var copy = new Student(Person, Education, _groupNumber);
        foreach (var t in _tests)
            if (t != null)
                copy.AddTests(new Test(t.Subject, t.IsPassed));
        foreach (var e in _exams)
            if (e != null)
                copy.AddExams(new Exam(e.Subject, e.Grade, e.ExamDate));
        return copy;
    }

    public void AddExams(params Exam[] exams)
    {
        if (exams.Length == 0) return;
        foreach (var e in exams) _exams.Add(e);
    }

    public void AddTests(params Test[] tests)
    {
        if (tests.Length == 0) return;
        foreach (var t in tests) _tests.Add(t);
    }

    public override string ToString()
    {
        var header =
            $"Student: [{base.ToString()}], Education={Education}, GroupNumber={_groupNumber}" +
            Environment.NewLine +
            $"TestsCount={_tests.Count}, ExamsCount={_exams.Count}";

        string testsText;
        if (_tests.Count == 0)
        {
            testsText = "Tests: (none)";
        }
        else
        {
            testsText = "Tests:" + Environment.NewLine;
            foreach (var t in _tests) testsText += " - " + t + Environment.NewLine;
            testsText = testsText.TrimEnd();
        }

        string examsText;
        if (_exams.Count == 0)
        {
            examsText = "Exams: (none)";
        }
        else
        {
            examsText = "Exams:" + Environment.NewLine;
            foreach (var e in _exams) examsText += " - " + e + Environment.NewLine;
            examsText = examsText.TrimEnd();
        }

        return header + Environment.NewLine + testsText + Environment.NewLine + examsText;
    }

    public new string ToShortString()
    {
        return $"Student: [{ToShortStringBase()}], Education={Education}, " +
               $"GroupNumber={_groupNumber}, AvgGrade={AverageGrade:F2}, " +
               $"TestsCount={_tests.Count}, ExamsCount={_exams.Count}";
    }

    private string ToShortStringBase() => base.ToShortString();

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not Student other) return false;
        if (!base.Equals(other)) return false;
        if (Education != other.Education) return false;
        if (_groupNumber != other._groupNumber) return false;
        if (_tests.Count != other._tests.Count) return false;
        if (_exams.Count != other._exams.Count) return false;
        for (var i = 0; i < _tests.Count; i++)
            if (!_tests[i].Equals(other._tests[i])) return false;
        for (var i = 0; i < _exams.Count; i++)
            if (!_exams[i].Equals(other._exams[i])) return false;
        return true;
    }

    public override int GetHashCode()
    {
        var hash = base.GetHashCode();
        hash = hash * 31 + Education.GetHashCode();
        hash = hash * 31 + _groupNumber.GetHashCode();
        foreach (var t in _tests) hash = hash * 31 + (t == null ? 0 : t.GetHashCode());
        foreach (var e in _exams) hash = hash * 31 + (e == null ? 0 : e.GetHashCode());
        return hash;
    }

    public static bool operator ==(Student? left, Student? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Student? left, Student? right) => !(left == right);

    public IEnumerable AllResults()
    {
        foreach (var t in _tests) yield return t;
        foreach (var e in _exams) yield return e;
    }

    public IEnumerable ExamsWithGradeGreaterThan(int minGrade)
    {
        foreach (var e in _exams)
            if (e.Grade > minGrade)
                yield return e;
    }

    public IEnumerable PassedResults()
    {
        foreach (var t in _tests)
            if (t.IsPassed) yield return t;
        foreach (var e in _exams)
            if (e.Grade > 2) yield return e;
    }

    public IEnumerable PassedTestsWithPassedExam()
    {
        foreach (var t in _tests)
        {
            if (!t.IsPassed) continue;
            foreach (var e in _exams)
                if (e.Subject == t.Subject && e.Grade > 2)
                {
                    yield return t;
                    break;
                }
        }
    }
}
