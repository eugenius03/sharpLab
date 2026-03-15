using System.Collections;

namespace SharpLab;

public class Student : Person, IEnumerable
{
    private readonly ArrayList _exams;
    private readonly ArrayList _tests;
    private int _groupNumber;

    public Student(Person person, Education education, int groupNumber)
        : base(person.FirstName, person.LastName, person.BirthDate)
    {
        Education = education;
        GroupNumber = groupNumber;
        _tests = new ArrayList();
        _exams = new ArrayList();
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

    public ArrayList Exams
    {
        get => _exams;
        init => _exams = value ?? new ArrayList();
    }

    public ArrayList Tests
    {
        get => _tests;
        init => _tests = value ?? new ArrayList();
    }

    public double AverageGrade
    {
        get
        {
            if (_exams.Count == 0) return 0.0;

            long sum = 0;
            foreach (Exam e in _exams)
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

        foreach (Test t in _tests)
            if (t != null)
                copy.AddTests(new Test(t.Subject, t.IsPassed));

        foreach (Exam e in _exams)
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
            foreach (Test t in _tests) testsText += " - " + t + Environment.NewLine;

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
            foreach (Exam e in _exams) examsText += " - " + e + Environment.NewLine;

            examsText = examsText.TrimEnd();
        }

        return header + Environment.NewLine + testsText + Environment.NewLine + examsText;
    }

    public new string ToShortString()
    {
        return $"Student: [{ToShortStringBase()}], Education={Education}, " +
               $"GroupNumber={_groupNumber}, AvgGrade={AverageGrade:F2}";
    }

    private string ToShortStringBase()
    {
        return base.ToShortString();
    }

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
        {
            var t1 = (Test)_tests[i]!;
            var t2 = (Test)other._tests[i]!;
            if (!t1.Equals(t2)) return false;
        }

        for (var i = 0; i < _exams.Count; i++)
        {
            var e1 = (Exam)_exams[i]!;
            var e2 = (Exam)other._exams[i]!;
            if (!e1.Equals(e2)) return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        var hash = base.GetHashCode();
        hash = hash * 31 + Education.GetHashCode();
        hash = hash * 31 + _groupNumber.GetHashCode();

        foreach (Test t in _tests) hash = hash * 31 + (t == null ? 0 : t.GetHashCode());

        foreach (Exam e in _exams) hash = hash * 31 + (e == null ? 0 : e.GetHashCode());

        return hash;
    }

    public static bool operator ==(Student? left, Student? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Student? left, Student? right)
    {
        return !(left == right);
    }

    public IEnumerable AllResults()
    {
        foreach (Test t in _tests)
            yield return t;

        foreach (Exam e in _exams)
            yield return e;
    }

    public IEnumerable ExamsWithGradeGreaterThan(int minGrade)
    {
        foreach (Exam e in _exams)
            if (e.Grade > minGrade)
                yield return e;
    }

    public IEnumerable PassedResults()
    {
        foreach (Test t in _tests)
            if (t.IsPassed)
                yield return t;

        foreach (Exam e in _exams)
            if (e.Grade > 2)
                yield return e;
    }

    public IEnumerable PassedTestsWithPassedExam()
    {
        foreach (Test t in _tests)
        {
            if (!t.IsPassed) continue;

            foreach (Exam e in _exams)
                if (e.Subject == t.Subject && e.Grade > 2)
                {
                    yield return t;
                    break;
                }
        }
    }
}