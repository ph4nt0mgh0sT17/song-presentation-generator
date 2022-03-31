using System;

namespace SongTheoryApplication.Exceptions;

public class SongCannotBeCreatedException : InvalidOperationException
{
    public SongCannotBeCreatedException(string? message) : base(message)
    {
    }

    public SongCannotBeCreatedException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}