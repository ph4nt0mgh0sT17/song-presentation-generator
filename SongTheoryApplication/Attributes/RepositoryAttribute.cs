using System;

namespace SongTheoryApplication.Attributes;

/// <summary>
/// A basic attribute that identifies class and interface as a repository.
/// If the service is to be injected both interface and class have to have this attribute attached.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class RepositoryAttribute : Attribute
{
}