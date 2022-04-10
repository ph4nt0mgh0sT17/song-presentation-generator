using SongTheoryApplication.Models;

namespace SongTheoryApplication.Repositories;

public interface ILocalSongRepository
{
    /// <summary>
    ///     Saves a new song into the local song repository.
    /// </summary>
    /// <param name="song">The <see cref="Song" /> to be saved.</param>
    void SaveSong(Song song);
}