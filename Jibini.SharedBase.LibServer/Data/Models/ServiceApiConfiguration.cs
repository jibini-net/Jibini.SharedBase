using System.Reflection;

namespace Jibini.SharedBase.Data.Models;

public class ServiceApiConfiguration
{
    public List<(Assembly, string[])> SearchNamespaces { get; set; } = new();
}
