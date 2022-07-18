using System;

namespace SongTheoryApplication.Exceptions;

public class SongDoesNotExist : InvalidOperationException
{
    public SongDoesNotExist() 
        : base($"Song does not exist.")
    {
    }
}