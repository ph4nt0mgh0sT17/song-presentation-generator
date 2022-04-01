using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Repositories;

public class LocalSongRepository : ILocalSongRepository
{
    public void SaveSong(Song song)
    {
        var songs = RetrieveAllSongs();
        songs.Add(song);

        var songsJsonText = JsonSerializer.Serialize(songs);

        var fileStream = new FileStream(Constants.Constants.SONGS_JSON_FILENAME, FileMode.Create);
        using (var streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(songsJsonText);
        }
    }

    private List<Song> RetrieveAllSongs()
    {
        if (!File.Exists(Constants.Constants.SONGS_JSON_FILENAME))
            return new List<Song>();

        var songsJsonText = RetrieveJsonFromSongsJsonFile();

        return JsonSerializer.Deserialize<List<Song>>(songsJsonText) ?? 
               throw new InvalidOperationException("The songs could not be retrieved.");
    }

    private string RetrieveJsonFromSongsJsonFile()
    {
        return File.ReadAllText(Constants.Constants.SONGS_JSON_FILENAME);
    }
}