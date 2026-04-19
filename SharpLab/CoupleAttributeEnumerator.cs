using System.Collections;

namespace SharpLab;

public class CoupleAttributeEnumerator(Type type) : IEnumerable<CoupleAttribute>, IEnumerator<CoupleAttribute>
{
    private readonly CoupleAttribute[] _attrs = (CoupleAttribute[])type.GetCustomAttributes(typeof(CoupleAttribute), false);
    private int _pos = -1;

    public bool MoveNext() => ++_pos < _attrs.Length;
    public void Reset()    => _pos = -1;
    public CoupleAttribute Current => _attrs[_pos];
    object IEnumerator.Current     => Current;
    public void Dispose() { }

    public IEnumerator<CoupleAttribute> GetEnumerator() { Reset(); return this; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
