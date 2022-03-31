using System.Collections.Generic;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Repositories;

public interface ILocalSongRepository
{
    /// <summary>
    /// Create a new song into the local song repository.
    /// </summary>
    /// <param name="song">The <see cref="Song"/> to be created.</param>
    void CreateSong(Song song);
}