namespace SharpLab;

public class Test
{
    public Test(string subject, bool isPassed)
    {
        Subject = subject;
        IsPassed = isPassed;
    }

    public Test()
        : this("SomeSubject", false)
    {
    }

    public string Subject { get; set; }
    public bool IsPassed { get; set; }

    public override string ToString()
    {
        return $"Subject={Subject}, IsPassed={IsPassed}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not Test other) return false;

        return Subject == other.Subject &&
               IsPassed == other.IsPassed;
    }

    public override int GetHashCode()
    {
        var hash = 17;
        hash = hash * 31 + (Subject == null ? 0 : Subject.GetHashCode());
        hash = hash * 31 + IsPassed.GetHashCode();
        return hash;
    }
}