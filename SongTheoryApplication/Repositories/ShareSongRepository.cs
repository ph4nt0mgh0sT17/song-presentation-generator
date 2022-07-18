using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using Firebase.Database;
using Firebase.Database.Query;
using SongTheoryApplication.Attributes;
using SongTheoryApplication.Models;

namespace SongTheoryApplication.Repositories;

[Repository]
public class ShareSongRepository : IShareSongRepository
{
    public async Task<string> SaveSongAsync(ShareSong? song)
    {
        Guard.IsNotNull(song);

        var firebaseClient = new FirebaseClient(
            "https://song-presentation-generator-default-rtdb.europe-west1.firebasedatabase.app",
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult("Cc0MrBW4v3LnZf9rq8agesobF5ToYM8E0soj0Mns")
            });

        var result = await firebaseClient
            .Child("shared-songs")
            .PostAsync(song);

        return result.Key;
    }

    public async Task DeleteSongAsync(string? shareSongId)
    {
        Guard.IsNotNull(shareSongId);

        var firebaseClient = new FirebaseClient(
            "https://song-presentation-generator-default-rtdb.europe-west1.firebasedatabase.app",
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult("Cc0MrBW4v3LnZf9rq8agesobF5ToYM8E0soj0Mns")
            });

        await firebaseClient
            .Child("shared-songs")
            .Child(shareSongId)
            .DeleteAsync();
    }

    public async Task<ShareSong> GetSongAsync(string? shareSongId)
    {
        Guard.IsNotNull(shareSongId);

        var firebaseClient = new FirebaseClient(
            "https://song-presentation-generator-default-rtdb.europe-west1.firebasedatabase.app",
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult("Cc0MrBW4v3LnZf9rq8agesobF5ToYM8E0soj0Mns")
            });

        return await firebaseClient
            .Child("shared-songs")
            .Child(shareSongId)
            .OnceSingleAsync<ShareSong>();
    }

    public async Task UpdateSongAsync(string? shareSongId, ShareSong? shareSong)
    {
        Guard.IsNotNull(shareSongId);
        Guard.IsNotNull(shareSong);

        var firebaseClient = new FirebaseClient(
            "https://song-presentation-generator-default-rtdb.europe-west1.firebasedatabase.app",
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult("Cc0MrBW4v3LnZf9rq8agesobF5ToYM8E0soj0Mns")
            });

        await firebaseClient
            .Child("shared-songs")
            .Child(shareSongId)
            .PutAsync(shareSong);
    }
}