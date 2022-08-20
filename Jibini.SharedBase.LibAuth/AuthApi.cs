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

    /// <summary>
    /// Actions for the "Account" API paths.
    /// </summary>
    public AccountPath Account { get; private set; }

    public AuthApi(string basePath)
    {
        Tenant = new(basePath);
        Account = new(basePath);
    }
}

/// <summary>
/// Actions for the "Tenant" API paths.
/// </summary>
public class TenantPath : IRetrievablePath<Tenant, SearchTenant, int>,
    IModifiablePath<Tenant, ISetTenant>,
    IDeletablePath<Tenant>
{
    public string BasePath { get; private set; }
    public string EndpointUri { get; private set; }

    public TenantPath(string basePath)
    {
        BasePath = basePath;
        EndpointUri = basePath.JoinUri("Tenant");
    }
}

/// <summary>
/// Actions for the "Account" API paths.
/// </summary>
public class AccountPath : IRetrievablePath<Account, SearchAccount, int>,
    IModifiablePath<Account, ISetAccount>,
    IDeletablePath<Account>
{
    public string BasePath { get; private set; }
    public string EndpointUri { get; private set; }

    public ApiPath<ISetAccountPassword, Account> SetPassword =>
        new(HttpMethod.Post, EndpointUri.JoinUri("SetPassword"));

    public AccountPath(string basePath)
    {
        BasePath = basePath;
        EndpointUri = basePath.JoinUri("Account");
    }
}
