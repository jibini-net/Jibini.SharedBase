namespace Jibini.SharedBase.Util.Services;

/// <summary>
/// Provides a way to check permissions before and during execution of a
/// procedure; allows the client to validate access prior to making a
/// request, and also the server to block requests for which the user lacks
/// proper roles.
/// </summary>
public interface IPermissionArbiter
{
    public enum PermissionState
    {
        Granted,
        Denied,
        RequiresAuthentication,
        RequiresRenewal
    }

    /// <summary>
    /// Checks whether an action is allowed given the current authentication
    /// and usage context.
    /// </summary>
    bool IsAllowed(string actionNode);
}
