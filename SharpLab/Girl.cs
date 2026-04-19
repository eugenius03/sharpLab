namespace SharpLab;

[Couple(Pair = "Student", Probability = 0.7)]
[Couple(Pair = "Botan",   Probability = 0.3)]
public class Girl : Human
{
    public override Gender Gender => Gender.Female;
    public string Patronymic { get; set; } = string.Empty;
}
