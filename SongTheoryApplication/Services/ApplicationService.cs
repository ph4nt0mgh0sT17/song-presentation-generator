using SongTheoryApplication.Attributes;

namespace SongTheoryApplication.Services;

[Service]
public class ApplicationService : IApplicationService
{
    /// <inheritdoc cref="IApplicationService.IsPowerPointInstalled"/>
    public bool IsPowerPointInstalled()
    {
        return true;
    }
}