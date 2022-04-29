using SongTheoryApplication.Models;

namespace SongTheoryApplication.Services;

public interface IPresentationGeneratorService
{
    /// <summary>
    ///     Generates a testing presentation with 'songTitle' and 'songText' into .pptx file
    ///     to given 'fileName'.
    /// </summary>
    /// <param name="songTitle">The title of the song.</param>
    /// <param name="songText">The text of the song.</param>
    /// <param name="fileName">The file name where the presentation will be saved.</param>
    void GenerateTestingPresentation(string? songTitle, string? songText, string fileName);

    void GeneratePresentation(Song? song, string fileName);
}