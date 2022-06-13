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

    public async Task DeleteSongAsync(string? songTitle)
    {
        Guard.IsNotNull(songTitle, nameof(songTitle));
        
        var songs = await RetrieveAllSongsAsync();
        songs = songs.Where(x => x.Title != songTitle).ToList();

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