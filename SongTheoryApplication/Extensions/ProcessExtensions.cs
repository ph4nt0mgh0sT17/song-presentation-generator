using System;
using System.Diagnostics;
using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Extensions;

public static class ProcessExtensions
{
    /// <summary>
    /// Starts the process.
    /// </summary>
    /// <param name="fileName">The file name of the file to be started.</param>
    /// <exception cref="InvalidOperationException">
    /// When the process cannot be started.
    /// </exception>
    public static void StartFileProcess(string? fileName)
    {
        Guard.IsNotNull(fileName);

        try
        {
            Process.Start(new ProcessStartInfo($"{fileName}") { UseShellExecute = true });
        }

        catch (Exception ex)
        {
            throw new InvalidOperationException("Cannot start the process.", ex);
        }
    }
}