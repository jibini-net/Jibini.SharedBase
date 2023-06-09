namespace Jibini.SharedBase.Data.Models;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class ApiActionAttribute : Attribute
{
    public string Method { get; set; } = "";
    public bool IsAnonymous { get; set; } = false;
    public string HasAll { get; set; } = "";
    public string HasAny { get; set; } = "";
}
