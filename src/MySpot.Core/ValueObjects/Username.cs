using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public class Username
{
    public string Value { get; }

    public Username(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length is > 30 or < 3)
            throw new InvalidUsernameException();

        Value = value;
    }

    public static implicit operator Username(string username) => new (username);
    public static implicit operator string(Username username) => username.Value;
}
