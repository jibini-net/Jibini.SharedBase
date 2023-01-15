namespace Jibini.SharedBase.Data.Models;

/// <summary>
/// Generic ordered pair which can be used in any model for any datapoint.
/// </summary>
public class Xy<TDomain, TRange>
{
    /// <summary>
    /// X- (or domain) value of the ordered pair.
    /// </summary>
    public TDomain? X { get; set; }

    /// <summary>
    /// Y- (or domain) value of the ordered pair.
    /// </summary>
    public TRange? Y { get; set; }

    public Xy(TDomain? x, TRange? y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Creates a pair which is suitable for JSON serialization from a tuple
    /// which is not.
    /// </summary>
    public static implicit operator Xy<TDomain, TRange>((TDomain, TRange) tuple) => new(tuple.Item1, tuple.Item2);
}

/// <summary>
/// Generic ordered pair which can be used in any model for any datapoint.
/// </summary>
public class Pair<T> : Xy<T, T>
{
    public Pair(T x, T y) : base(x, y) { }

    /// <summary>
    /// Creates a pair which is suitable for JSON serialization from a tuple
    /// which is not.
    /// </summary>
    public static implicit operator Pair<T>((T, T) tuple) => new(tuple.Item1, tuple.Item2);
}