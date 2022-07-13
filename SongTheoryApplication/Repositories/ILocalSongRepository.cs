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
    Task SaveSongAsync(Song song);

    Task UpdateSongAsync(string? id, Song? song);

    /// <summary>
    /// Deletes a song recognized by given songTitle.
    /// </summary>
    /// <param name="id">The ID of the song to be removed.</param>
    /// <exception cref="ArgumentNullException">Thrown when songTitle is null.</exception>
    Task DeleteSongAsync(string? id);

    /// <summary>
    /// Retrieves all songs stored in the system.
    /// </summary>
    /// <returns>The <see cref="List{T}"/> of <see cref="Song"/> objects.</returns>
    Task<List<Song>> RetrieveAllSongsAsync();
}