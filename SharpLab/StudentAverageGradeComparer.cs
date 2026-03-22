namespace SharpLab;

public class StudentAverageGradeComparer : IComparer<Student>
{
    public int Compare(Student? x, Student? y)
    {
        if (x is null && y is null) return 0;
        if (x is null) return -1;
        if (y is null) return 1;
        return x.AverageGrade.CompareTo(y.AverageGrade);
    }
}