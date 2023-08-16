using System.Text.RegularExpressions;

namespace Jibini.SharedBase.Util.Extensions;

/// <summary>
/// Convenience methods for formatting strings and string representations of
/// data, including dates, times, paths, and other plain text.
/// </summary>
public static class StringFormatExtensions
{
    /// <summary>
    /// Combines a base path string with one or more path segments.
    /// </summary>
    public static string JoinUri(this string it, params string[] segments)
    {
        return string.Join("/", segments
            .Select((it) => it.Trim('/'))
            .Prepend(it.TrimEnd('/')));
    }
        

    /// <summary>
    /// Combines a base path string with one or more path segments.
    /// </summary>
    public static string JoinPath(this string it, params string[] segments)
    {
        return Path.Combine(segments
            .Prepend(it)
            .ToArray());
    }

    /// <summary>
    /// Replaces only the first occurrence of a sequence in an input string.
    /// </summary>
    public static string ReplaceFirst(this string it, string find, string replace)
    {
        return new Regex(Regex.Escape(find))
            .Replace(it, replace, 1);
    }
}
