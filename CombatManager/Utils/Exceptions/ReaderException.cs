namespace CombatManager.Utils.Exceptions;

public class ReaderException : Exception
{
    public ReaderException()
    {
    }

    public ReaderException(string message) : base(message)
    {
    }

    public ReaderException(string message, Exception inner) : base(message, inner)
    {
    }
}