namespace SongTheoryApplication.Models;

public class PresentationFormatStyle
{
    public string Name { get; }

    public PresentationFormatStyle(string name)
    {
        Name = name;
    }

    public override bool Equals(object? obj)
    {
        if (obj is PresentationFormatStyle anotherFormatStyle)
        {
            return Name == anotherFormatStyle.Name;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}