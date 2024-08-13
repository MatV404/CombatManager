namespace CombatManager.Utils.Exceptions;

public class ExplorerException : Exception
{
    public ExplorerException()
    {
    }

    public ExplorerException(string message) : base(message)
    {
    }

    public ExplorerException(string message, Exception inner) : base(message, inner)
    {
    }
}