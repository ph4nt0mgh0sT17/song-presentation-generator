namespace SongTheoryApplication.Requests;

public class CreateSongRequest
{
    public string? SongTitle { get; set; }
    public string? SongInterpret { get; set; }
    public string? SongText { get; set; }
}