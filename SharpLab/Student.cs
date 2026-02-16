namespace SharpLab;

public class Student(Person person, Education education, int groupNumber)
{
    private Person _person = person;
    private Education _education = education;
    private int _groupNumber = groupNumber;
    private Exam[] _exams = Array.Empty<Exam>();

    public Student()
        : this(new Person(), Education.Bachelor, 101)
    {
    }

    public Person Person
    {
        get => _person;
        init => _person = value;
    }

    public Education Education
    {
        get => _education;
        init => _education = value;
    }

    public int GroupNumber
    {
        get => _groupNumber;
        init => _groupNumber = value;
    }

    public Exam[] Exams
    {
        get
        {
            var copy = new Exam[_exams.Length];
            Array.Copy(_exams, copy, _exams.Length);
            return copy;
        }
        init => _exams = value;
    }

    private double AverageGrade
    {
        get
        {
            if (_exams.Length == 0) return 0.0;
            long sum = 0;
            for (int i = 0; i < _exams.Length; i++)
                sum += _exams[i].Grade;
            return (double)sum / _exams.Length;
        }
    }

    public bool this[Education edu] => _education == edu;

    public void AddExams(params Exam[] exams)
    {
        if (exams.Length == 0) return;

        int oldLen = _exams.Length;
        int addLen = exams.Length;

        var newArr = new Exam[oldLen + addLen];
        Array.Copy(_exams, newArr, oldLen);
        Array.Copy(exams, 0, newArr, oldLen, addLen);

        _exams = newArr;
    }

    public override string ToString()
    {
        string header =
            $"Student: [{_person}], Education={_education}, GroupNumber={_groupNumber}\n" +
            $"ExamsCount={_exams.Length}";

        if (_exams.Length == 0) return header + "\nExams: (none)";

        string examsText = "Exams:\n";
        for (int i = 0; i < _exams.Length; i++)
        {
            examsText += $"  - {_exams[i]}\n";
        }

        return header + "\n" + examsText.TrimEnd();
    }

    public string ToShortString()
        =>
            $"Student: [{_person.ToShortString()}], Education={_education}, GroupNumber={_groupNumber}, AvgGrade={AverageGrade:F2}";
}