namespace SharpLab;

public class Person(string firstName, string lastName, DateTime birthDate)
    : IDateAndCopy, IComparable, IComparer<Person>
{
    protected DateTime _birthDate = birthDate;
    protected string _firstName = firstName;
    protected string _lastName = lastName;

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

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (obj is not Person other)
            throw new ArgumentException("Object is not a Person");
        return string.Compare(_lastName, other._lastName, StringComparison.Ordinal);
    }

    public int Compare(Person? x, Person? y)
    {
        if (x is null && y is null) return 0;
        if (x is null) return -1;
        if (y is null) return 1;
        return x._birthDate.CompareTo(y._birthDate);
    }

    public DateTime Date
    {
        get => _birthDate;
        init => _birthDate = value;
    }

    public virtual object DeepCopy()
    {
        return new Person(_firstName, _lastName, _birthDate);
    }

    public override string ToString()
    {
        return $"FirstName={_firstName}, LastName={_lastName}, BirthDate={_birthDate:yyyy-MM-dd}";
    }

    public string ToShortString()
    {
        return $"{_lastName} {_firstName}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not Person other) return false;

        return _firstName == other._firstName &&
               _lastName == other._lastName &&
               _birthDate == other._birthDate;
    }

    public override int GetHashCode()
    {
        var hash = 17;
        hash = hash * 31 + (_firstName == null ? 0 : _firstName.GetHashCode());
        hash = hash * 31 + (_lastName == null ? 0 : _lastName.GetHashCode());
        hash = hash * 31 + _birthDate.GetHashCode();
        return hash;
    }

    public static bool operator ==(Person? left, Person? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Person? left, Person? right)
    {
        return !(left == right);
    }
}