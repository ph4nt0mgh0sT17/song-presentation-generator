﻿using System;
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

    public async Task CreateSongAsync(CreateSongRequest? createSongRequest)
    {
        Guard.IsNotNull(createSongRequest);

        await ValidateSongDoesNotExist(createSongRequest);
        await InternalCreateSong(createSongRequest);
    }

    private async Task InternalCreateSong(CreateSongRequest createSongRequest)
    {
        var song = new Song(createSongRequest.SongTitle, createSongRequest.SongText, createSongRequest.IsSongShared, createSongRequest.SharedSongId);

        try
        {
            await _localSongRepository.SaveSongAsync(song);
        }

        catch (Exception ex)
        {
            throw new SongCannotBeCreatedException("Song cannot be created.", ex);
        }
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

        var song = new Song(editSongRequest.SongTitle, editSongRequest.SongText, editSongRequest.IsSongShared, editSongRequest.SharedSongId);

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
}