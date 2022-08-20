using Jibini.SharedBase.Util.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.RegularExpressions;

namespace Jibini.SharedBase.Util;

/// <summary>
/// Set of operations which can be performed to retrieve entities.
/// </summary>
public interface IRetrievablePath<T, TSearch, TKey>
{
    string EndpointUri { get; }

    ApiPath<object, T> Get =>
        new(HttpMethod.Get, EndpointUri.JoinUri("{}"));

    ApiPath<TSearch, List<T>> Search =>
        new(HttpMethod.Get, EndpointUri.JoinUri("Search"));
}

/// <summary>
/// Methods to generate valid fully qualified endpoint paths.
/// </summary>
public static class RetrievablePathExtensions
{
    public static ApiPath<object, T> Get<T, TSearch, TKey>(this IRetrievablePath<T, TSearch, TKey> it, TKey id) =>
        it.Get.FillIn(id);

    public static ApiPath<TSearch, List<T>> Search<T, TSearch, TKey>(this IRetrievablePath<T, TSearch, TKey> it, TSearch args) =>
        it.Search.AddArgs(args);
}

/// <summary>
/// Set of operations which can be performed to modify entities.
/// </summary>
public interface IModifiablePath<T, TSet>
{
    string EndpointUri { get; }

    ApiPath<TSet, T> Set =>
        new(HttpMethod.Post, EndpointUri);
    
    ApiPath<JsonPatchDocument, T> Patch =>
        new(HttpMethod.Patch, EndpointUri.JoinUri("{}"));
}

/// <summary>
/// Methods to generate valid fully qualified endpoint paths.
/// </summary>
public static class ModifiablePathExtensions
{
    public static ApiPath<TSet, T> Set<T, TSet>(this IModifiablePath<T, TSet> it) =>
        it.Set;

    public static ApiPath<JsonPatchDocument, T> Patch<T, TSet>(this IModifiablePath<T, TSet> it, object id) =>
        it.Patch.FillIn(id);
}

/// <summary>
/// Set of operations which can be performed to delete entities.
/// </summary>
public interface IDeletablePath<T>
{
    string EndpointUri { get; }

    ApiPath<object, T> Delete =>
        new(HttpMethod.Delete, EndpointUri.JoinUri("{}"));
}

/// <summary>
/// Methods to generate valid fully qualified endpoint paths.
/// </summary>
public static class DeletablePathExtensions
{
    public static ApiPath<object, T> Delete<T>(this IDeletablePath<T> it, object id) =>
        it.Delete.FillIn(id);
}

/// <summary>
/// A named path in the API which can be invoked. Each instance of a path has
/// one valid HTTP method and includes a fully qualified endpoint path.
/// </summary>
public class ApiPath<TArgs, TResult>
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
    public ApiPath<TArgs, TResult> FillIn(object? value) =>
        new(Method, Path.ReplaceFirst("{}", value?.ToString() ?? ""));

    /// <summary>
    /// Appends a parameter string to a path based on a provided model. Specify
    /// an interface type parameter to limit which fields are included.
    /// </summary>
    public ApiPath<TArgs, TResult> AddArgs(TArgs? args)
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
    public async Task<TResult?> InvokeAsync(TArgs? body = default)
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

        return json.ParseTo<TResult>();
    }

    /// <summary>
    /// Calls the API action via the correct method. If the specific method is
    /// <c>"GET"</c> or <c>"HEAD</c>," any provided body content is discarded.
    /// </summary>
    public async Task InvokeVoidAsync(TArgs? body = default) =>
        await InvokeAsync(body);
}
