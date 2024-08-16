using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DotTiled;

public class Color : IParsable<Color>, IEquatable<Color>
{
  public required byte R { get; set; }
  public required byte G { get; set; }
  public required byte B { get; set; }
  public byte A { get; set; } = 255;

  public static Color Parse(string s, IFormatProvider? provider)
  {
    TryParse(s, provider, out var result);
    return result ?? throw new FormatException($"Invalid format for TiledColor: {s}");
  }

  public static bool TryParse(
    [NotNullWhen(true)] string? s,
    IFormatProvider? provider,
    [MaybeNullWhen(false)] out Color result)
  {
    if (s is not null && !s.StartsWith('#'))
      return TryParse($"#{s}", provider, out result);

    // Format: #RRGGBB or #AARRGGBB
    if (s is null || s.Length != 7 && s.Length != 9 || s[0] != '#')
    {
      result = default;
      return false;
    }

    if (s.Length == 7)
    {
      result = new Color
      {
        R = byte.Parse(s[1..3], NumberStyles.HexNumber, provider),
        G = byte.Parse(s[3..5], NumberStyles.HexNumber, provider),
        B = byte.Parse(s[5..7], NumberStyles.HexNumber, provider)
      };
    }
    else
    {
      result = new Color
      {
        A = byte.Parse(s[1..3], NumberStyles.HexNumber, provider),
        R = byte.Parse(s[3..5], NumberStyles.HexNumber, provider),
        G = byte.Parse(s[5..7], NumberStyles.HexNumber, provider),
        B = byte.Parse(s[7..9], NumberStyles.HexNumber, provider)
      };
    }

    return true;
  }

  public bool Equals(Color? other)
  {
    if (other is null)
      return false;

    return R == other.R && G == other.G && B == other.B && A == other.A;
  }

  public override bool Equals(object? obj) => obj is Color other && Equals(other);

  public override int GetHashCode() => HashCode.Combine(R, G, B, A);

  public override string ToString() => $"#{A:x2}{R:x2}{G:x2}{B:x2}";
}
