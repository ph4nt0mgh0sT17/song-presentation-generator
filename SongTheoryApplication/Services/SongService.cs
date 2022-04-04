using System;
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
            _localSongRepository.SaveSong(song);
        }

        catch (Exception ex)
        {
            throw new SongCannotBeCreatedException("Song cannot be created.", ex);
        }
    }
}