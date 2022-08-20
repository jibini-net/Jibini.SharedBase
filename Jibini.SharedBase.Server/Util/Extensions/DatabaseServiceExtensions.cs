using System.Data.SqlClient;

namespace Jibini.SharedBase.Util.Extensions;

/// <summary>
/// Simple extensions for manipulating datasets within the database service.
/// </summary>
public static class DatabaseServiceExtensions
{
    /// <summary>
    /// Converts the provided object to a set of key-value argument pairs to
    /// provide to a stored procedure.
    /// </summary>
    public static void AddArgs<TArgs>(this SqlParameterCollection coll, TArgs? args)
    {
        if (args is null) return;

        var type = typeof(TArgs);
        var props = type.GetProperties();

        foreach (var prop in props)
        {
            var value = prop.GetValue(args);
            coll.AddWithValue($"@{prop.Name}", value);
        }
    }
}
