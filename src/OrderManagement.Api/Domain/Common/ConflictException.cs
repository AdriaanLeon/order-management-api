namespace OrderManagement.Api.Domain.Common;

public sealed class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}