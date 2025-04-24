using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DotTiled;

/// <summary>
/// Represents a Tiled color.
/// </summary>
public class TiledColor : IParsable<TiledColor>, IEquatable<TiledColor>
{
  /// <summary>
  /// The red component of the color.
  /// </summary>
  public required byte R { get; set; }

  /// <summary>
  /// The green component of the color.
  /// </summary>
  public required byte G { get; set; }

  /// <summary>
  /// The blue component of the color.
  /// </summary>
  public required byte B { get; set; }

  /// <summary>
  /// The alpha component of the color.
  /// </summary>
  public byte A { get; set; } = 255;

  /// <summary>
  /// Attempts to parse the specified string into a <see cref="TiledColor"/>. Expects strings in the format <c>#RRGGBB</c> or <c>#AARRGGBB</c>.
  /// The leading <c>#</c> is optional.
  /// </summary>
  /// <param name="s">A string value to parse into a <see cref="TiledColor"/></param>
  /// <param name="provider">An object that supplies culture-specific information about the format of s.</param>
  /// <returns>The parsed <see cref="TiledColor"/></returns>
  /// <exception cref="FormatException">Thrown in case the provided string <paramref name="s"/> is not in a valid format.</exception>
  public static TiledColor Parse(string s, IFormatProvider provider)
  {
    _ = TryParse(s, provider, out var result);
    return result ?? throw new FormatException($"Invalid format for TiledColor: {s}");
  }

  /// <summary>
  /// Attempts to parse the specified string into a <see cref="TiledColor"/>. Expects strings in the format <c>#RRGGBB</c> or <c>#AARRGGBB</c>.
  /// The leading <c>#</c> is optional.
  /// </summary>
  /// <param name="s">A string value to parse into a <see cref="TiledColor"/></param>
  /// <param name="provider">An object that supplies culture-specific information about the format of s.</param>
  /// <param name="result">When this method returns, contains the parsed <see cref="TiledColor"/> or <c>null</c> on failure.</param>
  /// <returns><c>true</c> if <paramref name="s"/> was successfully parsed; otherwise, <c>false</c>.</returns>
  public static bool TryParse(
    [NotNullWhen(true)] string s,
    IFormatProvider provider,
    [MaybeNullWhen(false)] out TiledColor result)
  {
    if (s is not null && !s.StartsWith('#'))
      return TryParse($"#{s}", provider, out result);

    // Format: #RRGGBB or #AARRGGBB
    if (s is null || (s.Length != 7 && s.Length != 9) || s[0] != '#')
    {
      result = default;
      return false;
    }

    if (s.Length == 7)
    {
      result = new TiledColor
      {
        R = byte.Parse(s[1..3], NumberStyles.HexNumber, provider),
        G = byte.Parse(s[3..5], NumberStyles.HexNumber, provider),
        B = byte.Parse(s[5..7], NumberStyles.HexNumber, provider)
      };
    }
    else
    {
      result = new TiledColor
      {
        A = byte.Parse(s[1..3], NumberStyles.HexNumber, provider),
        R = byte.Parse(s[3..5], NumberStyles.HexNumber, provider),
        G = byte.Parse(s[5..7], NumberStyles.HexNumber, provider),
        B = byte.Parse(s[7..9], NumberStyles.HexNumber, provider)
      };
    }

    return true;
  }

  /// <inheritdoc/>
  public bool Equals(TiledColor other)
  {
    if (other is null)
      return false;

    return R == other.R && G == other.G && B == other.B && A == other.A;
  }

  /// <inheritdoc/>
  public override bool Equals(object obj) => obj is TiledColor other && Equals(other);

  /// <inheritdoc/>
  public override int GetHashCode() => HashCode.Combine(R, G, B, A);

  /// <inheritdoc/>
  public override string ToString() => $"#{A:x2}{R:x2}{G:x2}{B:x2}";
}
