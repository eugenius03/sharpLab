using System;

namespace SharpLab;

public delegate void StudentListHandler(object source, StudentListHandlerEventArgs args);

public class StudentListHandlerEventArgs(string collectionName, string changeInfo, Student? student) : EventArgs
{
    public string   CollectionName  { get; init; } = collectionName;
    public string   ChangeInfo      { get; init; } = changeInfo;
    public Student? ChangedStudent  { get; init; } = student;

    public StudentListHandlerEventArgs(string collectionName, string changeInfo)
        : this(collectionName, changeInfo, null) { }

    public override string ToString() =>
        $"EventArgs {{ Collection='{CollectionName}', Change='{ChangeInfo}', " +
        $"Student={ChangedStudent?.ToShortString() ?? "<null>"} }}";
}