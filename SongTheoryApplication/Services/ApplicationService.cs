using System;
using SongTheoryApplication.Attributes;

namespace SongTheoryApplication.Services;

[Service(true)]
public class ApplicationService : IApplicationService
{
    private bool? _isPowerPointInstalled;

    public bool IsPowerPointInstalled
    {
        get
        {
            if (_isPowerPointInstalled != null)
                return (bool)_isPowerPointInstalled;

            _isPowerPointInstalled = Type.GetTypeFromProgID("Powerpoint.Application") != null;
            return (bool)_isPowerPointInstalled;
        }
    }
}