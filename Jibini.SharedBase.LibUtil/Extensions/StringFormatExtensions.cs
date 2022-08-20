using System.Text.RegularExpressions;

namespace Jibini.SharedBase.Util.Extensions;

/// <summary>
/// Convenience methods for formatting strings and string representations of
/// data, including dates, times, paths, and other plain text.
/// </summary>
public static class StringFormatExtensions
{
    /// <summary>
    /// Combines a base path string with one or more new path segments.
    /// </summary>
    public static string JoinUri(this string it, params string[] segments) =>
        string.Join("/", new[] { it.TrimEnd('/') }
            .Concat(segments.Select((it) => it.Trim('/'))));

    /// <summary>
    /// Combines a base path string with one or more new path segments.
    /// </summary>
    public static string JoinPath(this string it, params string[] segments) =>
        Path.Combine(new[] { it }
            .Concat(segments)
            .ToArray());

    public static string ReplaceFirst(this string it, string find, string replace) =>
        new Regex(Regex.Escape(find))
            .Replace(it, replace, 1);
}
