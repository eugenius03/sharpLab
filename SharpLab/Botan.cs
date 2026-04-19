namespace SharpLab;

[Couple(Pair = "Girl",       Probability = 0.7, ChildType = "SmartGirl")]
[Couple(Pair = "PrettyGirl", Probability = 1.0, ChildType = "PrettyGirl")]
[Couple(Pair = "SmartGirl",  Probability = 0.8, ChildType = "Book")]
public class Botan : Human
{
    public override Gender Gender => Gender.Male;
}
