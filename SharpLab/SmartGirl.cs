namespace SharpLab;

[Couple(Pair = "Student", Probability = 0.2)]
[Couple(Pair = "Botan",   Probability = 0.5)]
public sealed class SmartGirl : Human
{
    public override Gender Gender => Gender.Female;
}
