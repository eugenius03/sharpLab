using System.Collections;

namespace SharpLab;

public class StudentEnumerator : IEnumerator
{
    private readonly ArrayList _subjects;
    private int _position = -1;

    public StudentEnumerator(Student student)
    {
        _subjects = new ArrayList();

        var examSubjects = new ArrayList();
        foreach (Exam e in student.Exams)
            if (e?.Subject != null && !examSubjects.Contains(e.Subject))
                examSubjects.Add(e.Subject);

        foreach (Test t in student.Tests)
            if (t?.Subject != null &&
                examSubjects.Contains(t.Subject) &&
                !_subjects.Contains(t.Subject))
                _subjects.Add(t.Subject);
    }

    public object Current
    {
        get
        {
            if (_position < 0 || _position >= _subjects.Count)
                throw new InvalidOperationException();

            return _subjects[_position]!;
        }
    }

    public bool MoveNext()
    {
        _position++;
        return _position < _subjects.Count;
    }

    public void Reset()
    {
        _position = -1;
    }
}