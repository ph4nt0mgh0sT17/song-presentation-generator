using SongTheoryApplication.Models;
using System.Threading.Tasks;

namespace SongTheoryApplication.Repositories;

public interface IShareSongRepository
{
    /// <summary>
    /// Saves a new song into the firebase shared-songs repository.
    /// </summary>
    /// <param name="song">The <see cref="ShareSong" /> to be saved.</param>
    Task SaveSongAsync(string? shareSongId, ShareSong? song);


    Task DeleteSongAsync(string? shareSongId);

    Task<ShareSong> GetSongAsync(string? shareSongId);

    Task UpdateSongAsync(string? shareSongId, ShareSong? shareSong);
}