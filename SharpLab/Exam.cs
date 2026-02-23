namespace SharpLab;

public class Exam(string subject, int grade, DateTime examDate)
{
    public Exam()
        : this("Math", 0, DateTime.Today)
    {
    }

    public string Subject { get; init; } = subject;
    public int Grade { get; set; } = grade;
    public DateTime ExamDate { get; init; } = examDate;

    public override string ToString()
    {
        return $"Subject={Subject}, Grade={Grade}, ExamDate={ExamDate:yyyy-MM-dd}";
    }
}