namespace SharpLab;

public class Person(string firstName, string lastName, DateTime birthDate)
{
    public Person()
        : this("Ivan", "Petrenko", new DateTime(2000, 1, 1))
    {
    }

    public string FirstName { get; init; } = firstName;
    public string LastName { get; init; } = lastName;
    public DateTime BirthDate { get; init; } = birthDate;

    public override string ToString()
    {
        return $"FirstName={FirstName}, LastName={LastName}, BirthDate={BirthDate:yyyy-MM-dd}";
    }

    public string ToShortString()
    {
        return FirstName + " " + LastName;
    }
}