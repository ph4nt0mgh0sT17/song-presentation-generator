using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Constants;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Repositories;

[Repository]
public class LocalSongRepository : ILocalSongRepository
{
    public async Task SaveSongAsync(Song song)
    {
        var songs = await RetrieveAllSongsAsync();
        songs.Add(song);

        var songsJsonText = JsonSerializer.Serialize(songs);

        var fileStream = new FileStream(ApplicationConstants.SONGS_JSON_FILENAME, FileMode.Create);

        await using var streamWriter = new StreamWriter(fileStream);
        await streamWriter.WriteAsync(songsJsonText);
    }

    public async Task UpdateSongAsync(string? id, Song? song)
    {
        Guard.IsNotNull(id);
        Guard.IsNotNull(song);

        var songs = await RetrieveAllSongsAsync();

        var existingSong = songs.Find(x => x.Id == id);
        existingSong.Title = song.Title;
        existingSong.Text = song.Text;
        existingSong.IsSongShared = song.IsSongShared;
        existingSong.SharedSongId = song.SharedSongId;

        var songsJsonText = JsonSerializer.Serialize(songs);

        var fileStream = new FileStream(ApplicationConstants.SONGS_JSON_FILENAME, FileMode.Create);

        await using var streamWriter = new StreamWriter(fileStream);
        await streamWriter.WriteAsync(songsJsonText);
    }

    public async Task DeleteSongAsync(string? id)
    {
        Guard.IsNotNull(id);
        
        var songs = await RetrieveAllSongsAsync();
        songs = songs.Where(x => x.Id != id).ToList();

        var songsJsonText = JsonSerializer.Serialize(songs);

        var fileStream = new FileStream(ApplicationConstants.SONGS_JSON_FILENAME, FileMode.Create);
        await using var streamWriter = new StreamWriter(fileStream);
        await streamWriter.WriteAsync(songsJsonText);
    }

    public async Task<List<Song>> RetrieveAllSongsAsync()
    {
        if (!File.Exists(ApplicationConstants.SONGS_JSON_FILENAME))
            return new List<Song>();

        var songsJsonText = await RetrieveJsonFromSongsJsonFileAsync();

        return JsonSerializer.Deserialize<List<Song>>(songsJsonText) ??
               throw new InvalidOperationException("The songs could not be retrieved.");
    }

    private static async Task<string> RetrieveJsonFromSongsJsonFileAsync()
    {
        return await File.ReadAllTextAsync(ApplicationConstants.SONGS_JSON_FILENAME);
    }
}