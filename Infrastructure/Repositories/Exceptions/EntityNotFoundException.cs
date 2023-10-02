namespace jonas.Infrastructure.Repositories.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) :  base(message) { }
}
