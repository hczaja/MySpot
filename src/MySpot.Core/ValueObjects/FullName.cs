using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public class FullName
{
    public string Value { get; }

    public FullName(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length is > 100 or < 3)
            throw new InvalidFullNameException();

        Value = value;
    }
    
    public static implicit operator FullName(string fullname) => new (fullname);
    public static implicit operator string(FullName fullname) => fullname.Value;
}
