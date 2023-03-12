using System.DirectoryServices;
using System.Text;

namespace Jibini.SharedBase.Util.Services;

/// <summary>
/// Quick extension to better interpret results sets from LDAP server.
/// </summary>
public static class PropExtensions
{
    public static string? GetOrDefault(this ResultPropertyCollection that, string key)
    {
        if (!that.Contains(key))
        {
            return null;
        }
        if (that[key].Count < 1)
        {
            return null;
        }
        return that[key][0]?.ToString();
    }
}

/// <summary>
/// Base fields used to read accounts from organizational unit.
/// </summary>
public class ActiveDirectoryAccount
{
    public string AccountName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string CommonName { get; set; } = "";
}

/// <summary>
/// Provides access to the configured LDAP account server, allowing users to
/// attempt authentication via a domain-wide shared account server.
/// </summary>
public class ActiveDirectoryService
{
    private readonly IConfiguration config;

    public ActiveDirectoryService(IConfiguration config)
    {
        this.config = config;
    }

    /// <summary>
    /// If either username or password is missing, use default account.
    /// </summary>
    public bool UseServiceAccount => string.IsNullOrEmpty(config.GetValue<string>("ActiveDirectory:Username"))
        || string.IsNullOrEmpty(config.GetValue<string>("ActiveDirectory:Password"));

    /// <summary>
    /// Finds the username to use, depending on configuration state.
    /// </summary>
    public string ConnectAsAccount => UseServiceAccount
        ? Environment.UserName
        : config.GetValue<string>("ActiveDirectory:Username")!;

    /// <exception cref="Exception">If the service should not run.</exception>
    private void ValidateEnabled()
    {
        // This particular build is targeted to Windows OS; shouldn't error
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            throw new Exception("Active directory services are supported only on Windows");
        }
        // May prevent accidental usage of incorrect config values
        if (config.GetValue<bool>("ActiveDirectory:UseActiveDirectory") != true)
        {
            throw new Exception("Active directory services must be enabled in settings");
        }
    }

    /// <summary>
    /// Returns unfiltered list of objects visible to connected account.
    /// </summary>
    public IEnumerable<ActiveDirectoryAccount> ListUsers() => FindUsers("");

    /// <summary>
    /// Filters the list of objects to find one with a particular account CN.
    /// </summary>
    public ActiveDirectoryAccount? FindUser(string commonName) => FindUsers(commonName, false).FirstOrDefault();

    /// <summary>
    /// Takes plain-text credentials and verifies them against the configured
    /// LDAP server by attempting to connect and execute a query as that user.
    /// </summary>
    public ActiveDirectoryAccount? Authenticate(string username, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }
            // Query for "the user" account as "the user"
            var results = FindUsers(username, false, username, password);

            // Attempt to connect using provided credentials
            return results.Count() switch
            {
                // Should never be zero
                0 => null,
                1 => results.First(),
                _ => throw new Exception($"There are multiple domain accounts with the common name '{username}'"),
            };
        } catch (DirectoryServicesCOMException ex)
        {
            // Check for specific incorrect username and password error
            if (unchecked(ex.ErrorCode == (int)0x8007052eL))
            {
                return null;
            } else
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Filters the list of objects to find users whose CNs contain a sequence.
    /// </summary>
    public IEnumerable<ActiveDirectoryAccount> FindUsers(string cnFilter) => FindUsers(cnFilter, true);

    /// <summary>
    /// Shared query function to format and execute LDAP queries posed above.
    /// </summary>
    private List<ActiveDirectoryAccount> FindUsers(string cnFilter, bool substring, string? username = null, string? password = null)
    {
        ValidateEnabled();
        if (!substring && string.IsNullOrEmpty(cnFilter))
        {
            return new();
        }

        var encoded = EscapeLdapSearchFilter(cnFilter);
        if (substring)
        {
            encoded = $"*{encoded}*";
        }
        var userQuery = string.IsNullOrEmpty(cnFilter) ? "" : $"(sAMAccountName={encoded})";

        if (!UseServiceAccount && string.IsNullOrEmpty(username))
        {
            // Fill in missing credentials with configured values
            username = config.GetValue<string>("ActiveDirectory:Username")!;
            password = config.GetValue<string>("ActiveDirectory:Password")!;
        }

        using var rootEntry = new DirectoryEntry(config.GetValue<string>("ActiveDirectory:LdapUri")!, username, password, AuthenticationTypes.Secure);
        using var searcher = new DirectorySearcher(rootEntry, $"(&(objectClass=User)(objectCategory=Person){userQuery})");
        
        // Convert searcher results to basic data model
        return searcher.FindAll()
            .Cast<SearchResult>()
            .Select((it) => new ActiveDirectoryAccount()
            {
                AccountName = it.Properties.GetOrDefault("samaccountname")!,
                // First name is always required, coalesce if necessary
                FirstName = it.Properties.GetOrDefault("givenName") ?? it.Properties.GetOrDefault("samaccountname")!,
                LastName = it.Properties.GetOrDefault("sn") ?? "",
                CommonName = it.Properties.GetOrDefault("cn")!
            })
            .ToList();
    }

    /// <summary>
    /// Escapes any special characters which would allow injecting unintended
    /// content into LDAP queries via filter values.
    /// </summary>
    private static string EscapeLdapSearchFilter(string searchFilter)
    {
        if (string.IsNullOrEmpty(searchFilter))
        {
            return "";
        }
        var escape = new StringBuilder();

        for (int i = 0; i < searchFilter.Length; ++i)
        {
            char current = searchFilter[i];
            switch (current)
            {
                case '\\':
                    escape.Append(@"\5c");
                    break;
                case '*':
                    escape.Append(@"\2a");
                    break;
                case '(':
                    escape.Append(@"\28");
                    break;
                case ')':
                    escape.Append(@"\29");
                    break;
                case '\u0000':
                    escape.Append(@"\00");
                    break;
                case '/':
                    escape.Append(@"\2f");
                    break;
                default:
                    escape.Append(current);
                    break;
            }
        }

        return escape.ToString();
    }
}