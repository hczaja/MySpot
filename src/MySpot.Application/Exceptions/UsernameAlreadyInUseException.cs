using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class UsernameAlreadyInUseException : CustomException
{
    public UsernameAlreadyInUseException() 
        : base("Username already in use.")
    { }
}