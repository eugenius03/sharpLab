namespace SharpLab;

public abstract class Human : IHasName
{
    private static readonly string[] Names =
        { "Олена", "Марія", "Катерина", "Оксана", "Ірина", "Наталія", "Аліна", "Соломія" };

    private static readonly Random Rng = new();

    public string Name { get; set; } = string.Empty;
    public abstract Gender Gender { get; }
    
    public string GetName() =>
        Names[Rng.Next(Names.Length)];
}
