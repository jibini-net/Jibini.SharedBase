using Newtonsoft.Json;

namespace Jibini.SharedBase.Util.Extensions;

/// <summary>
/// Quick access to JSON parsing utilities to convert text to objects and vice
/// versa. Also allows conversion between types through an, albeit inefficient,
/// sequence of serialization then deserialization.
/// </summary>
public static class ObjectTypeExtensions
{
    /// <summary>
    /// Attempts to convert the input JSON to the specified type.
    /// </summary>
    public static T ParseTo<T>(this string json) =>
        JsonConvert.DeserializeObject<T>(json ?? "null");

    /// <summary>
    /// Attempts to serialize the input object to JSON.
    /// </summary>
    public static string ToJson(this object input) =>
        JsonConvert.SerializeObject(input);

    /// <summary>
    /// Serializes the object to JSON then parses it again into the specified
    /// output type.
    /// </summary>
    public static TOut ConvertTo<TIn, TOut>(this TIn input) =>
        input.ToJson().ParseTo<TOut>();
}
