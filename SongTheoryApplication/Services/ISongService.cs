using System.Threading.Tasks;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

public interface ISongService
{
    /// <summary>
    /// Creates a new song in the system.
    /// </summary>
    /// <param name="createSongRequest">The request for creating new song.</param>
    Task CreateSongAsync(CreateSongRequest? createSongRequest);

    Task UpdateSongAsync(EditSongRequest? editSongRequest);

    /// <summary>
    /// Deletes a song identified by its title.
    /// </summary>
    /// <param name="deleteSongRequest">The request containing the title of the song to be deleted.</param>
    Task DeleteSongAsync(DeleteSongRequest? deleteSongRequest);
}