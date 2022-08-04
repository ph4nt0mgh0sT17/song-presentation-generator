using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Exceptions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

[Service]
public class SongService : ISongService
{
    private readonly ILocalSongRepository _localSongRepository;

    public SongService(ILocalSongRepository localSongRepository)
    {
        _localSongRepository = localSongRepository;
    }

    public async Task<Song> CreateSongAsync(CreateSongRequest? createSongRequest)
    {
        Guard.IsNotNull(createSongRequest);

        return await InternalCreateSong(createSongRequest);
    }

    private async Task<Song> InternalCreateSong(CreateSongRequest createSongRequest)
    {
        var song = new Song(createSongRequest.SongTitle, createSongRequest.SongText, createSongRequest.SongSource, createSongRequest.IsSongShared, createSongRequest.SharedSongId, createSongRequest.IsSongDownloaded);

        try
        {
            await _localSongRepository.SaveSongAsync(song);
        }

        catch (Exception ex)
        {
            throw new SongCannotBeCreatedException("Song cannot be created.", ex);
        }

        return song;
    }

    private async Task ValidateSongDoesNotExist(CreateSongRequest createSongRequest)
    {
        var allSongs = await _localSongRepository.RetrieveAllSongsAsync();

        if (allSongs.Any(song => song.Title == createSongRequest.SongTitle))
        {
            throw new SongAlreadyExistsException(createSongRequest.SongTitle);
        }
    }

    public async Task UpdateSongAsync(EditSongRequest? editSongRequest)
    {
        Guard.IsNotNull(editSongRequest);

        var song = new Song(editSongRequest.SongTitle, editSongRequest.SongText, editSongRequest.SongSource, editSongRequest.IsSongShared, editSongRequest.SharedSongId);

        try
        {
            await _localSongRepository.UpdateSongAsync(editSongRequest.Id, song);
        }

        catch (Exception ex)
        {
            throw new SongCannotBeCreatedException("Song cannot be created.", ex);
        }
    }

    public async Task DeleteSongAsync(DeleteSongRequest? deleteSongRequest)
    {
        Guard.IsNotNull(deleteSongRequest);

        await _localSongRepository.DeleteSongAsync(deleteSongRequest.Id);
    }

    public async Task<Song> FindSongAsync(Func<Song, bool>? predicate)
    {
        Guard.IsNotNull(predicate);

        var allSongs = await _localSongRepository.RetrieveAllSongsAsync();

        var song = allSongs.Where(predicate).First();

        return song;
    }
}