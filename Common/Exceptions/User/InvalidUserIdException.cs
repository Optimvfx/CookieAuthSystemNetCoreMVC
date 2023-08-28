namespace Common.Exceptions.User;

public class InvalidUserIdException : ArgumentException
{
    public readonly Guid? UserId = null;
    
    public InvalidUserIdException()
    {
        
    }

    public InvalidUserIdException(Guid userId)
    {
        UserId = userId;
    }
}