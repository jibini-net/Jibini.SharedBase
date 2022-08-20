using Jibini.SharedBase.Util.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.RegularExpressions;

namespace Jibini.SharedBase.Util;

/// <summary>
/// Set of operations which can be performed to retrieve entities.
/// </summary>
public interface IRetrievablePath<T, TSearch, TKey>
{
    string EndpointUri { get; }

    ApiPath Get => new(HttpMethod.Get, EndpointUri.JoinUri("{}"));

    ApiPath Search => new(HttpMethod.Get, EndpointUri.JoinUri("Search"));
}

/// <summary>
/// Methods to generate valid fully qualified endpoint paths.
/// </summary>
public static class RetrievablePathExtensions
{
    public static ApiPath Get<T, TSearch, TKey>(this IRetrievablePath<T, TSearch, TKey> it, TKey id) =>
        it.Get.FillIn(id);

    public static ApiPath Search<T, TSearch, TKey>(this IRetrievablePath<T, TSearch, TKey> it, TSearch args) =>
        it.Search.AddArgs(args);
}

/// <summary>
/// Set of operations which can be performed to modify entities.
/// </summary>
public interface IModifiablePath<T>
{
    string EndpointUri { get; }

    ApiPath Set => new(HttpMethod.Post, EndpointUri);
    
    ApiPath Patch => new(HttpMethod.Patch, EndpointUri.JoinUri("{}"));
}

/// <summary>
/// Methods to generate valid fully qualified endpoint paths.
/// </summary>
public static class ModifiablePathExtensions
{
    public static ApiPath Set<T>(this IModifiablePath<T> it) =>
        it.Set;

    public static ApiPath Patch<T>(this IModifiablePath<T> it, object id) =>
        it.Patch.FillIn(id);
}

/// <summary>
/// Set of operations which can be performed to delete entities.
/// </summary>
public interface IDeletablePath<T>
{
    string EndpointUri { get; }

    ApiPath Delete => new(HttpMethod.Delete, EndpointUri.JoinUri("{}"));
}

/// <summary>
/// Methods to generate valid fully qualified endpoint paths.
/// </summary>
public static class DeletablePathExtensions
{
    public static ApiPath Delete<T>(this IDeletablePath<T> it, object id) =>
        it.Delete.FillIn(id);
}

/// <summary>
/// A named path in the API which can be invoked. Each instance of a path has
/// one valid HTTP method and includes a fully qualified endpoint path.
/// </summary>
public class ApiPath
{
    /// <summary>
    /// Which HTTP method is used to invoke this endpoint path.
    /// </summary>
    public HttpMethod Method { get; private set; }

    /// <summary>
    /// The fully qualified path of the API endpoint, with any empty path
    /// parameters replaced with a placeholder ("<c>{}</c>").
    /// </summary>
    public string Path { get; private set; }

    public ApiPath(HttpMethod method, string path)
    {
        Method = method;
        Path = path;
    }

    /// <summary>
    /// Replaces the first empty placeholder ("<c>{}</c>") in the path with a
    /// real parameter value.
    /// </summary>
    public ApiPath FillIn(object? value)
    {
        var regex = new Regex(Regex.Escape("{}"));
        var newPath = regex.Replace(Path, value?.ToString() ?? "", 1);

        return new(Method, newPath);
    }

    /// <summary>
    /// Appends a parameter string to a path based on a provided model. Specify
    /// an interface type parameter to limit which fields are included.
    /// </summary>
    public ApiPath AddArgs<TArgs>(TArgs? args)
    {
        if (args is null)
            return this;

        var values = new Dictionary<string, object?>();
        var fields = typeof(TArgs)
            .GetFields()
            .ToDictionary((it) => it.Name, (it) => it.GetValue(args)?.ToString());

        return new(Method, QueryHelpers.AddQueryString(Path, fields));
    }

    /// <summary>
    /// Calls the API action via the correct method. If the specific method is
    /// <c>"GET"</c> or <c>"HEAD</c>," any provided body content is discarded.
    /// </summary>
    public async Task<T?> InvokeAsync<T>(object? body = null)
    {
        using var client = new HttpClient();
        var result = await client.SendAsync(
            new HttpRequestMessage(Method, Path)
            {
                Content = (Method == HttpMethod.Get || Method == HttpMethod.Head)
                    ? null
                    : new StringContent(body.ToJson() ?? "")
            });

        var json = await result.Content.ReadAsStringAsync();
        result.EnsureSuccessStatusCode();

        return json.ParseTo<T>();
    }

    /// <summary>
    /// Calls the API action via the correct method. If the specific method is
    /// <c>"GET"</c> or <c>"HEAD</c>," any provided body content is discarded.
    /// </summary>
    public async Task InvokeVoidAsync<T>(object? body) =>
        await InvokeAsync<T>(body);
}
