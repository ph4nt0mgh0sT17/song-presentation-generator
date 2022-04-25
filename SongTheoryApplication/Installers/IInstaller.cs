using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SongTheoryApplication.Installers;

/// <summary>
/// A simple interface for all installer for dependency injection services.
/// </summary>
public interface IInstaller
{
    void ConfigureServices(IServiceCollection services);
}