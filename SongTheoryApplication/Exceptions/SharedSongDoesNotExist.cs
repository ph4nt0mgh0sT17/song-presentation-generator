using System;

namespace SongTheoryApplication.Exceptions;

public class SharedSongDoesNotExist : InvalidOperationException
{
    public SharedSongDoesNotExist(string? sharedSongId) 
        : base($"Shared song with ID: '{sharedSongId ?? "unknown ID"}' does not exist.")
    {
    }
}