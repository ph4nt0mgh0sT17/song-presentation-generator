using System;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Exceptions;
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

    public async Task ShareSong(ShareSongRequest? shareSongRequest)
    {
        Guard.IsNotNull(shareSongRequest);

        await _shareSongRepository.SaveSongAsync(
            shareSongRequest.ShareSongId, 
            new ShareSong(shareSongRequest.SongTitle,
            shareSongRequest.SongText, shareSongRequest.SongSource)
       );
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

        // TODO: Check share song really exists
        if (shareSong == null)
            throw new InvalidOperationException($"The share song with id {shareSong} does not exist.");

        await _songService.CreateSongAsync(new CreateSongRequest(shareSong.Title, shareSong.Title, shareSong.Source, false, sharedSongId, true));
    }

    public async Task UpdateSongAsync(string? sharedSongId, ShareSongRequest? updateShareSongRequest)
    {
        Guard.IsNotNull(sharedSongId);
        Guard.IsNotNull(updateShareSongRequest);

        await _shareSongRepository.UpdateSongAsync(
            sharedSongId,
            new ShareSong(
                updateShareSongRequest.SongTitle,
                updateShareSongRequest.SongText,
                updateShareSongRequest.SongSource
            )
        );
    }

    public async Task UpdateDownloadedSongAsync(string? sharedSongId)
    {
        Guard.IsNotNull(sharedSongId);

        var sharedSong = await _shareSongRepository.GetSongAsync(sharedSongId);

        if (sharedSong == null)
            throw new SharedSongDoesNotExist(sharedSongId);

        var song = await _songService.FindSongAsync(song => song.SharedSongId == sharedSongId);

        if (song == null)
            throw new SongDoesNotExist();

        await _songService.UpdateSongAsync(
            new EditSongRequest(
                song.Id, sharedSong.Title, sharedSong.Text, sharedSong.Source, false, song.SharedSongId, true
            )
        );
    }
}