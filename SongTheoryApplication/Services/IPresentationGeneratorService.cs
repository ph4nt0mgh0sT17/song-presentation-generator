using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

public interface IPresentationGeneratorService
{
    void GeneratePresentation(PresentationGenerationRequest? presentationGenerationRequest, string fileName);
}