using System.Collections.Generic;
using System.Text;

namespace SharpLab;

public class Journal
{
    private readonly List<JournalEntry> _entries = new();

    public void OnStudentCountChanged(object source, StudentListHandlerEventArgs args)
    {
        _entries.Add(new JournalEntry(
            args.CollectionName,
            args.ChangeInfo,
            args.ChangedStudent?.ToShortString() ?? "<null>"));
    }

    public void OnStudentReferenceChanged(object source, StudentListHandlerEventArgs args)
    {
        _entries.Add(new JournalEntry(
            args.CollectionName,
            args.ChangeInfo,
            args.ChangedStudent?.ToShortString() ?? "<null>"));
    }

    public override string ToString()
    {
        if (_entries.Count == 0) return "  (порожній)";
        var sb = new StringBuilder();
        for (var i = 0; i < _entries.Count; i++)
            sb.AppendLine($"  {i + 1,2}. {_entries[i]}");
        return sb.ToString().TrimEnd();
    }
}