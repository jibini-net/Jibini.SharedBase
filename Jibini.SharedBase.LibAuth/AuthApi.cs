using Jibini.SharedBase.Util;
using Jibini.SharedBase.Util.Extensions;

namespace Jibini.SharedBase.Auth;

/// <summary>
/// Object bound to the paths of the API for a specific base path.
/// </summary>
public class AuthApi
{
    /// <summary>
    /// Actions for the "Tenant" API paths.
    /// </summary>
    public TenantPath Tenant { get; private set; }

    public AuthApi(string basePath)
    {
        Tenant = new(basePath);
    }
}

/// <summary>
/// Actions for the "Tenant" API paths.
/// </summary>
public class TenantPath : IRetrievablePath<ITenant, ISearchTenant, int>,
    IModifiablePath<ITenant>,
    IDeletablePath<ITenant>
{
    public string BasePath { get; private set; }
    public string EndpointUri { get; private set; }

    public TenantPath(string basePath)
    {
        BasePath = basePath;
        EndpointUri = basePath.JoinUri("Tenant");
    }
}
