namespace Jibini.SharedBase.Data;

public class Account : IAccount
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string? LastName { get; set; }
    public string Email { get; set; } = "";
    public string? CellNumber { get; set; }
    public string? HomeNumber { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
    public DateTime? PasswordSet { get; set; }
    public int? PasswordDuration { get; set; }
    public bool PasswordValid { get; set; }
    public bool PasswordExpired { get; set; }
    public DateTime? LastLogin { get; set; }
    public bool Enabled { get; set; } = true;
    public DateTime? EnabledToggled { get; set; }
    public DateTime? Deleted { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

public interface IAccount : ISetAccount, ISetAccountPassword
{
    public new int Id { get; set; }
    public string Name { get; set; }
    public DateTime? PasswordSet { get; set; }
    public bool PasswordValid { get; set; }
    public bool PasswordExpired { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime? EnabledToggled { get; set; }
    public DateTime? Deleted { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

public interface ISetAccount
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string? CellNumber { get; set; }
    public string? HomeNumber { get; set; }
    public int? PasswordDuration { get; set; }
    public bool Enabled { get; set; }
}

public interface ISetAccountPassword
{
    public int Id { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
}