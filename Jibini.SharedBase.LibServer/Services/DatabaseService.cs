using Jibini.SharedBase.Util.Extensions;
using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Jibini.SharedBase.Util.Services;

/// <summary>
/// Singleton database service which allows access to call stored procedures in
/// configured databases. Configure SQL Server database connection strings in
/// the application's settings file and reference them by name.
/// </summary>
public class DatabaseService
{
    private readonly IConfiguration config;

    public DatabaseService(IConfiguration config)
    {
        this.config = config;
    }

    /// <summary>
    /// Invokes a stored procedure with optional arguments, returning no value.
    /// </summary>
    public void CallProc<TArgs>(string name,
        TArgs? args = default,
        string db = "DefaultConnection")
    {
        using var conn = new SqlConnection(config.GetConnectionString(db));
        conn.Open();

        using var proc = conn.CreateCommand();
        proc.CommandText = name;
        proc.CommandType = System.Data.CommandType.StoredProcedure;
        proc.Parameters.AddArgs(args);

        proc.ExecuteNonQuery();
    }

    /// <summary>
    /// Invokes a stored procedure with optional arguments, returning a set of
    /// each result row's first columns parsed to JSON.
    /// </summary>
    public IEnumerable<TResult?> CallProcForJson<TArgs, TResult>(string name,
        TArgs? args = default,
        string db = "DefaultConnection")
    {
        using var conn = new SqlConnection(config.GetConnectionString(db));
        conn.Open();

        using var proc = conn.CreateCommand();
        proc.CommandText = name;
        proc.CommandType = CommandType.StoredProcedure;
        proc.Parameters.AddArgs(args);

        using var results = proc.ExecuteReader();

        while (results.Read())
        { 
            yield return results[0] is null
                ? default
                : results[0]!.ToString()!.ParseTo<TResult>();
        }

        yield break;
    }
}

