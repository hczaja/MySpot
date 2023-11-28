namespace MySpot.Core.ValueObjects;

public class UserId
{
    public Guid Value { get; private set; } 

    public UserId(Guid value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator Guid(UserId id) => id.Value;
    public static implicit operator UserId(Guid id) => new (id);
}
