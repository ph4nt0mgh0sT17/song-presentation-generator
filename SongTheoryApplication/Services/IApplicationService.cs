namespace SongTheoryApplication.Services;

public interface IApplicationService
{
    /// <summary>
    /// Checks if the Microsoft PowerPoint product is installed in the client computer.
    /// </summary>
    /// <returns>The state if the PowerPoint is installed or not.</returns>
    bool IsPowerPointInstalled();
}