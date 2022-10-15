namespace CloudStorage.DAL.Exceptions;

public class FileDuplicationException : Exception
{
    public FileDuplicationException(string? message) : base(message)
    {
    }
}