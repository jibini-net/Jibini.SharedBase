using Jibini.SharedBase.Auth;
using Jibini.SharedBase.Util.Services;

namespace Jibini.SharedBase.Data;

/// <summary>
/// Repository service for interacting with records of a certain category or
/// type. Dependency injected into more complex services to provide them with
/// simple access to the database.
/// </summary>
public class AccountRepository
{
    private readonly DatabaseService database;

    public AccountRepository(DatabaseService database)
    {
        this.database = database;
    }

    /// <summary>
    /// Argument schema for <c>dbo.[Account_Delete]</c> stored procedure.
    /// </summary>
    private class Account_Delete_Args
    {
        public int? Id { get; set; }
    }

    /// <summary>
    /// Deletes the specified record from the database, throwing an error if it
    /// is not found.
    /// </summary>
    public IAccount Delete(int id) =>
        database.CallProcForJson<Account_Delete_Args, Account>("dbo.[Account_Delete]",
                new()
                {
                    Id = id
                })
            .Single()!;

    /// <summary>
    /// Argument schema for <c>dbo.[Account_Get]</c> stored procedure.
    /// </summary>
    private class Account_Get_Args
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
    }

    /// <summary>
    /// Gets the single record associated with the provided ID, or nothing if it
    /// doesn't exist.
    /// </summary>
    public IAccount? Get(int id) =>
        database.CallProcForJson<Account_Get_Args, Account>("dbo.[Account_Get]",
                new()
                {
                    Id = id
                })
            ?.SingleOrDefault();

    /// <summary>
    /// Gets the single record associated with the provided ID, or nothing if it
    /// doesn't exist.
    /// </summary>
    public IAccount? Get(string email) =>
        database.CallProcForJson<Account_Get_Args, Account>("dbo.[Account_Get]",
                new()
                {
                    Email = email
                })
            ?.SingleOrDefault();

    /// <summary>
    /// Argument schema for <c>dbo.[Account_LogIn]</c> stored procedure.
    /// </summary>
    private class Account_LogIn_Args
    {
        public int? Id { get; set; }
    }

    /// <summary>
    /// Reports to the database that the specified user is logged in.
    /// </summary>
    public IAccount LogIn(int id) =>
        database.CallProcForJson<Account_Delete_Args, Account>("dbo.[Account_LogIn]",
                new()
                {
                    Id = id
                })
            .Single()!;

    /// <summary>
    /// Modifies the values in the database if the specified record already
    /// exists, or creates a new record otherwise.
    /// </summary>
    public IAccount Set(ISetAccount account) =>
        database.CallProcForJson<ISetAccount, Account>("dbo.[Account_Set]",
                account)
            .Single()!;

    /// <summary>
    /// Saves only the password fields from an account detail object.
    /// </summary>
    public IAccount SetPassword(ISetAccountPassword account) =>
        database.CallProcForJson<ISetAccountPassword, Account>("dbo.[Account_SetPassword]",
                account)
            .Single()!;
}