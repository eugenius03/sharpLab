namespace SharpLab;

public interface IDateAndCopy
{
    DateTime Date { get; init; }
    object DeepCopy();
}