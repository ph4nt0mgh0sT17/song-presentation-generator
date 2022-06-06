using System;
using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Exceptions;

public class SongTextParseException : InvalidOperationException
{
    public string ApplicationErrorText { get; }

    public SongTextParseException(string? message, string? applicationErrorText) : base(message)
    {
        Guard.IsNotNull(applicationErrorText, nameof(applicationErrorText));
        ApplicationErrorText = applicationErrorText;
    }

    public SongTextParseException(string? message, Exception? innerException, string? applicationErrorText)
        : base(message, innerException)
    {
        Guard.IsNotNull(applicationErrorText, nameof(applicationErrorText));
        ApplicationErrorText = applicationErrorText;
    }
}