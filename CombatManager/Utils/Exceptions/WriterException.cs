namespace CombatManager.Utils.Exceptions;

public class WriterException : Exception
{
    public WriterException()
    {
    }

    public WriterException(string message) : base(message)
    {
    }

    public WriterException(string message, Exception inner) : base(message, inner)
    {
    }
}