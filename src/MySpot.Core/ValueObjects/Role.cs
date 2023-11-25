namespace MySpot.Core.ValueObjects;

public class Role
{
    public string Value { get; }

    public Role(string value)
    {
        Value = value;
    }

    public static Role User() => new Role("User");

    public static implicit operator Role(string role) => new (role);
    public static implicit operator string(Role role) => role.Value;
}
