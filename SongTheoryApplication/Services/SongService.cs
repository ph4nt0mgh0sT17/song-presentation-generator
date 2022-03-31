using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Documents;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Exceptions;
using SongTheoryApplication.Models;
using SongTheoryApplication.Repositories;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

public class SongService : ISongService
{
    private readonly ILocalSongRepository _localSongRepository;

    public SongService(ILocalSongRepository localSongRepository)
    {
        _localSongRepository = localSongRepository;
    }

    public void CreateSong(CreateSongRequest createSongRequest)
    {
        var song = new Song(createSongRequest.SongTitle, createSongRequest.SongText);

        try
        {
            _localSongRepository.CreateSong(song);
        }

        catch (Exception ex)
        {
            throw new SongCannotBeCreatedException("Song cannot be created.", ex);
        }
    }
}