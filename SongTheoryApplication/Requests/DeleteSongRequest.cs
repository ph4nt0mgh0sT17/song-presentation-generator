using CommunityToolkit.Diagnostics;

namespace SongTheoryApplication.Requests;

public class DeleteSongRequest
{
    public string Id { get; }

    public DeleteSongRequest(string? id)
    {
        Guard.IsNotNull(id);
        Id = id;
    }
}