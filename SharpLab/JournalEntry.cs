namespace SharpLab;

public class JournalEntry(string collectionName, string changeType, string studentInfo)
{
    public string CollectionName { get; init; } = collectionName;
    public string ChangeType     { get; init; } = changeType;
    public string StudentInfo    { get; init; } = studentInfo;

    public override string ToString() =>
        $"[{CollectionName}] {ChangeType} => {StudentInfo}";
}