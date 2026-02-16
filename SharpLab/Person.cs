namespace SharpLab;

public class Person(string firstName, string lastName, DateTime birthDate)
{
    private string _firstName = firstName;
    private string _lastName = lastName;
    private DateTime _birthDate = birthDate;

    public Person()
        : this("Ivan", "Petrenko", new DateTime(2000, 1, 1))
    {
    }

    public string FirstName
    {
        get => _firstName;
        init => _firstName = value;
    }

    public string LastName
    {
        get => _lastName;
        init => _lastName = value;
    }

    public DateTime BirthDate
    {
        get => _birthDate;
        init => _birthDate = value;
    }

    public override string ToString()
        => $"FirstName={_firstName}, LastName={_lastName}, BirthDate={_birthDate:yyyy-MM-dd}";

    public string ToShortString()
        => $"{_lastName} {_firstName}";
}