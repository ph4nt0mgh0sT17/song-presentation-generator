using System;

namespace SongTheoryApplication.Exceptions;

public class SongAlreadyExistsException : InvalidOperationException
{
    public SongAlreadyExistsException(string? songTitle) : base($"Song '{songTitle ?? "unknown name"}' cannot be saved because it already exists.")
    {
    }
}