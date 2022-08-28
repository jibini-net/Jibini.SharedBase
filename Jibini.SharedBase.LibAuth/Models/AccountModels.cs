using System.ComponentModel.DataAnnotations;

namespace Jibini.SharedBase.Auth;

/// <summary>
/// Model for all fields returned by a result set containing accounts.
/// </summary>
public class Account : IAccount
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    [MaxLength(50, ErrorMessage = "First cannot exceed 50 characters.")]
    [Required(ErrorMessage = "First name is required.")]
    public string FirstName { get; set; } = "";
    [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    public string? LastName { get; set; }
    [MaxLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; } = "";
    [MaxLength(50, ErrorMessage = "Phone number cannot exceed 50 characters.")]
    public string? CellNumber { get; set; }
    [MaxLength(50, ErrorMessage = "Phone number cannot exceed 50 characters.")]
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

/// <summary>
/// Interface of all fields returned by a result set containing accounts.
/// </summary>
public interface IAccount : ISetAccount, ISetAccountPassword
{
    new int Id { get; set; }
    string Name { get; set; }
    DateTime? PasswordSet { get; set; }
    bool PasswordValid { get; set; }
    bool PasswordExpired { get; set; }
    DateTime? LastLogin { get; set; }
    DateTime? EnabledToggled { get; set; }
    DateTime? Deleted { get; set; }
    DateTime Created { get; set; }
    DateTime Updated { get; set; }
}

/// <summary>
/// Interface of fields used to modify user account details.
/// </summary>
public interface ISetAccount
{
    int Id { get; set; }
    string FirstName { get; set; }
    string? LastName { get; set; }
    string Email { get; set; }
    string? CellNumber { get; set; }
    string? HomeNumber { get; set; }
    int? PasswordDuration { get; set; }
    bool Enabled { get; set; }
}

/// <summary>
/// Model for set of fields which can be used to filter user accounts.
/// </summary>
public class SearchAccount
{
    public string Name { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string CellNumber { get; set; } = "";
    public string HomeNumber { get; set; } = "";
    public bool? PasswordValid { get; set; }
    public bool? PasswordExpired { get; set; }
    public bool? Enabled { get; set; }
}

/// <summary>
/// Interface of fields used to set an account's current password.
/// </summary>
public interface ISetAccountPassword
{
    int Id { get; set; }
    string? PasswordHash { get; set; }
    string? PasswordSalt { get; set; }
}

/// <summary>
/// Model for set of fields used to set an account's current password.
/// </summary>
public class SetAccountPassword : ISetAccountPassword
{
    public int Id { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
}