namespace MySpot.Api.Exceptions;

public sealed class InvalidEntityIdException : CustomException
{
    public Guid Id { get; }

    public InvalidEntityIdException(Guid id) 
        : base($"Guid: {id} is invalid!")
    {
        Id = id;
    }

}
