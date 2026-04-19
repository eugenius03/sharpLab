namespace SharpLab;

[Couple(Pair = "Student", Probability = 0.4)]
[Couple(Pair = "Botan",   Probability = 0.1)]
public sealed class PrettyGirl : Human
{
    public override Gender Gender => Gender.Female;
    public string Patronymic { get; set; } = string.Empty;
}
