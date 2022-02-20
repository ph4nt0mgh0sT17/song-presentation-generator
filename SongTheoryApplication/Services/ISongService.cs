using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

public interface ISongService
{
    void CreateSong(CreateSongRequest createSongRequest);
}