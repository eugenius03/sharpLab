namespace SharpLab;

public class Exam(string subject, int grade, DateTime examDate) : IDateAndCopy
{
    public Exam()
        : this("Math", 0, DateTime.Today)
    {
    }

    public string Subject { get; set; } = subject;
    public int Grade { get; set; } = grade;
    public DateTime ExamDate { get; set; } = examDate;

    public DateTime Date
    {
        get => ExamDate;
        init => ExamDate = value;
    }

    public object DeepCopy()
    {
        return new Exam(Subject, Grade, ExamDate);
    }

    public override string ToString()
    {
        return $"Subject={Subject}, Grade={Grade}, ExamDate={ExamDate:yyyy-MM-dd}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not Exam other) return false;

        return Subject == other.Subject &&
               Grade == other.Grade &&
               ExamDate == other.ExamDate;
    }

    public override int GetHashCode()
    {
        var hash = 17;
        hash = hash * 31 + (Subject == null ? 0 : Subject.GetHashCode());
        hash = hash * 31 + Grade.GetHashCode();
        hash = hash * 31 + ExamDate.GetHashCode();
        return hash;
    }

    public static bool operator ==(Exam? left, Exam? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Exam? left, Exam? right)
    {
        return !(left == right);
    }
}