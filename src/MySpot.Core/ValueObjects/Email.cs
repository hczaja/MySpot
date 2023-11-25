using MySpot.Core.Exceptions;

namespace MySpot.Core.ValueObjects;

public class Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrEmpty(value) && value.Length > 100)
            throw new InvalidEmailException();

        Value = value;
    }

    public static implicit operator Email(string email) => new (email);
    public static implicit operator string(Email email) => email.Value;
}
