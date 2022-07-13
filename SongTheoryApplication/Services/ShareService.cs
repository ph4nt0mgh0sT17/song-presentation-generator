using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Models;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

[Service]
public class ShareService : IShareService
{
    private readonly IShareSongRepository _shareSongRepository;
    private readonly ISongService _songService;

    public ShareService(IShareSongRepository shareSongRepository, ISongService songService)
    {
        _shareSongRepository = shareSongRepository;
        _songService = songService;
    }

    public async Task<string> ShareSong(ShareSongRequest? shareSongRequest)
    {
        Guard.IsNotNull(shareSongRequest);

        return await _shareSongRepository.SaveSongAsync(new ShareSong(shareSongRequest.SongTitle,
            shareSongRequest.SongText));
    }

    public async Task DeleteSongAsync(string? sharedSongId)
    {
        Guard.IsNotNull(sharedSongId);
        await _shareSongRepository.DeleteSongAsync(sharedSongId);
    }

    public async Task AddSharedSong(string? sharedSongId)
    {
        Guard.IsNotNull(sharedSongId);

        var shareSong = await _shareSongRepository.GetSongAsync(sharedSongId);

        await _songService.CreateSongAsync(new CreateSongRequest(shareSong.Title, shareSong.Title, true, sharedSongId));
    }
}