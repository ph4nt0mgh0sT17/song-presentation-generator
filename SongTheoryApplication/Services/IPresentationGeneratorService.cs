using System.Collections.Generic;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

public interface IPresentationGeneratorService
{
    void GeneratePresentation(PresentationGenerationRequest? presentationGenerationRequest, string fileName);


    void GenerateMultipleSongsPresentation(
        List<PresentationGenerationRequest>? presentationGenerationRequests, 
        bool addEmptySlidesBetweenSongs, 
        string fileName
   );
}