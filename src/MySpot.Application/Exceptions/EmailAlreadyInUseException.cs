using MySpot.Core.Exceptions;

namespace MySpot.Application.Exceptions;

public class EmailAlreadyInUseException : CustomException
{
    public EmailAlreadyInUseException() 
        : base("Email already in use.")
    { }
}