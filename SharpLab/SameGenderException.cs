namespace SharpLab;

public class SameGenderException(Human first, Human second) : Exception(
    $"Зустрілись два {(first.Gender == Gender.Male ? "чоловіки" : "жінки")}: " +
    $"{first.GetType().Name} ({first.Name}) та {second.GetType().Name} ({second.Name})")
{
    public Human First  { get; } = first;
    public Human Second { get; } = second;
}
