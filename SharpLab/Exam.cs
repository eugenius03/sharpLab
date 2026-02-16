namespace SharpLab;

public class Exam(string subject, int grade, DateTime examDate)
{
    public string Subject { get; set; } = subject;
    public int Grade { get; set; } = grade;
    public DateTime ExamDate { get; set; } = examDate;

    public Exam()
        : this("Math", 0, DateTime.Today)
    {
    }

    public override string ToString()
        => $"Subject={Subject}, Grade={Grade}, ExamDate={ExamDate:yyyy-MM-dd}";
}