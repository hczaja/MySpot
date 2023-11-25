namespace MySpot.Core.Exceptions;

public sealed class InvalidUsernameException : CustomException
{
    public InvalidUsernameException() 
        : base($"Username is invalid.")
    { }
}
