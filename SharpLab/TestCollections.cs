using System.Diagnostics;

namespace SharpLab;

public class TestCollections
{
    private readonly Dictionary<Person, Student> _personDict;
    private readonly List<Person> _personList;
    private readonly Dictionary<string, Student> _stringDict;
    private readonly List<string> _stringList;

    public TestCollections(int count)
    {
        _personList = new List<Person>(count);
        _stringList = new List<string>(count);
        _personDict = new Dictionary<Person, Student>(count);
        _stringDict = new Dictionary<string, Student>(count);

        for (var i = 0; i < count; i++)
        {
            var student = GenerateStudent(i);
            var key = student.Person;
            var keyStr = key.ToString();

            _personList.Add(key);
            _stringList.Add(keyStr);
            _personDict[key] = student;
            _stringDict[keyStr] = student;
        }
    }

    public static Student GenerateStudent(int n)
    {
        var person = new Person(
            $"FirstName{n}",
            $"LastName{n}",
            new DateTime(2000, 1, 1).AddDays(n));

        var groupNumber = 101 + Math.Abs(n) % 598; // always in range [101, 699]
        var student = new Student(person, (Education)(Math.Abs(n) % 3), groupNumber);
        student.AddExams(new Exam($"Subject{n}", n % 5 + 1, DateTime.Today));
        student.AddTests(new Test($"Subject{n}", n % 2 == 0));
        return student;
    }


    public void MeasureSearchTimes()
    {
        var count = _personList.Count;

        var cases = new (string Label, Person Key)[]
        {
            ("First  ", _personList[0]),
            ("Middle ", _personList[count / 2]),
            ("Last   ", _personList[count - 1]),
            ("Missing", GenerateStudent(-1).Person)
        };

        Console.WriteLine();
        Console.WriteLine($"{"Element",-10} {"List<Person>",-18} {"List<string>",-18} " +
                          $"{"Dict<P>.Key",-18} {"Dict<P>.Value",-18} {"Dict<str>.Key",-18}");
        Console.WriteLine($"{"(ticks)",-10} {"Contains",-18} {"Contains",-18} " +
                          $"{"ContainsKey",-18} {"ContainsValue",-18} {"ContainsKey",-18}");
        Console.WriteLine(new string('-', 98));

        foreach (var (label, key) in cases)
        {
            var keyStr = key.ToString();

            var t1 = MeasureTicks(() => _personList.Contains(key));
            var t2 = MeasureTicks(() => _stringList.Contains(keyStr));
            var t3 = MeasureTicks(() => _personDict.ContainsKey(key));

            // For ContainsValue we need the actual Student value (or any non-null Student for "Missing")
            var valStudent = _personDict.TryGetValue(key, out var s) ? s : GenerateStudent(-1);
            var t4 = MeasureTicks(() => _personDict.ContainsValue(valStudent));

            var t5 = MeasureTicks(() => _stringDict.ContainsKey(keyStr));

            Console.WriteLine($"{label,-10} {t1,16}   {t2,16}   {t3,16}   {t4,16}   {t5,16}");
        }
    }

    private static long MeasureTicks(Action action)
    {
        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        return sw.ElapsedTicks;
    }
}