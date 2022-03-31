namespace SongTheoryApplication.Services;

public interface IPresentationGeneratorService
{
    void GenerateTestingPresentation(string? songTitle, string? songText);
}