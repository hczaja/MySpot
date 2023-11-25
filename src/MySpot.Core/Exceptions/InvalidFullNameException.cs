namespace MySpot.Core.Exceptions;

public sealed class InvalidFullNameException : CustomException
{
    public InvalidFullNameException() 
        : base($"User full name is invalid.")
    { }
}
