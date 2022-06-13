using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Repositories;

public interface ILocalSongRepository
{
    /// <summary>
    /// Saves a new song into the local song repository.
    /// </summary>
    /// <param name="song">The <see cref="Song" /> to be saved.</param>
    void SaveSong(Song song);

    /// <summary>
    /// Deletes a song recognized by given songTitle.
    /// </summary>
    /// <param name="songTitle">The title name of the song to be removed.</param>
    /// <exception cref="ArgumentNullException">Thrown when songTitle is null.</exception>
    Task DeleteSongAsync(string? songTitle);

    Task<List<Song>> RetrieveAllSongsAsync();
}