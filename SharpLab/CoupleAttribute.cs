namespace SharpLab;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class CoupleAttribute : Attribute
{
    public string Pair      { get; set; } = string.Empty;
    public double Probability { get; set; }
    public string ChildType { get; set; } = string.Empty;
}
