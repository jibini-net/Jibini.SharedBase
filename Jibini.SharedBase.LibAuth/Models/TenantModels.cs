namespace Jibini.SharedBase.Auth;

/// <summary>
/// Model for user groups who use the application simultaneously.
/// </summary>
public class Tenant : ITenant
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}

/// <summary>
/// Model for user groups who use the application simultaneously.
/// </summary>
public interface ITenant : ISetTenant
{
    new int Id { get; set; }
    new string Name { get; set; }
    DateTime Created { get; set; }
    DateTime Modified { get; set; }
}

/// <summary>
/// Interface of fields which are saved while modifying application tenants.
/// </summary>
public interface ISetTenant
{
    int Id { get; set; }
    string Name { get; set; }
}

/// <summary>
/// Model for set of fields which can be used to filter application tenants.
/// </summary>
public class SearchTenant
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
}
