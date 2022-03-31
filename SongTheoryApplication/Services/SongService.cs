using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Documents;
using CommunityToolkit.Diagnostics;
using SongTheoryApplication.Models;
using SongTheoryApplication.Requests;

namespace SongTheoryApplication.Services;

public class SongService : ISongService
{
    public void CreateSong(CreateSongRequest createSongRequest)
    {
        Guard.IsNotNull(createSongRequest, nameof(createSongRequest));
        if (createSongRequest == null)
            throw new ArgumentNullException(nameof(createSongRequest));

        var song = new Song
        {
            Title = createSongRequest.SongTitle,
            Text = createSongRequest.SongText
        };

        var songs = RetrieveAllSongsFromJson();
        songs.Add(song);

        var songsJsonText = JsonSerializer.Serialize(songs);

        var fileStream = new FileStream(Constants.Constants.SONGS_JSON_FILENAME, FileMode.Create);
        using (var streamWriter = new StreamWriter(fileStream))
        {
            streamWriter.Write(songsJsonText);
        }
    }

    private List<Song> RetrieveAllSongsFromJson()
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