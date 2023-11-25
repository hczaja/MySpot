namespace MySpot.Core.Exceptions;

public sealed class InvalidEmailException : CustomException
{
    public InvalidEmailException() 
        : base($"User email is invalid.")
    { }
}
