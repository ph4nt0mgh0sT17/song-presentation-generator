using System;

namespace SongTheoryApplication.Exceptions;

public class SharedSongAlreadyExistsException : InvalidOperationException
{
    public SharedSongAlreadyExistsException(string? sharedSongId) 
        : base($"Shared song with ID: '{sharedSongId ?? "unknown ID"}' already exists.")
    {
    }
}