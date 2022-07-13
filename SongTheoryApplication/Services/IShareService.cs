using System.Threading.Tasks;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

public interface IShareService
{
    Task<string> ShareSong(ShareSongRequest? shareSongRequest);
    Task DeleteSongAsync(string? sharedSongId);
    Task AddSharedSong(string? sharedSongId);
}