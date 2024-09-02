using System;

namespace DotTiled;

/// <summary>
/// Represents a value that may or may not be present.
/// </summary>
/// <typeparam name="T">The type of the optionally present value.</typeparam>
public class Optional<T>
{
  private readonly T _value;

  /// <summary>
  /// Gets a value indicating whether the current <see cref="Optional{T}"/> object has a value.
  /// </summary>
  public bool HasValue { get; }

  /// <summary>
  /// Gets the value of the current <see cref="Optional{T}"/> object if it has been set; otherwise, throws an exception.
  /// </summary>
  public T Value => HasValue ? _value : throw new InvalidOperationException("Value is not set");

  /// <summary>
  /// Initializes a new instance of the <see cref="Optional{T}"/> class with the specified value.
  /// </summary>
  /// <param name="value">The value to be set.</param>
  public Optional(T value)
  {
    _value = value;
    HasValue = true;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Optional{T}"/> class with no value.
  /// </summary>
  public Optional()
  {
    _value = default!;
    HasValue = false;
  }

  /// <summary>
  /// Implicitly converts a value to an <see cref="Optional{T}"/> object.
  /// </summary>
  /// <param name="value">The value to be converted.</param>
  public static implicit operator Optional<T>(T value)
  {
    if (value is null)
      return new();

    return new(value);
  }

  /// <summary>
  /// Implicitly converts an <see cref="Optional{T}"/> object to a value.
  /// </summary>
  /// <param name="optional">The <see cref="Optional{T}"/> object to be converted.</param>
  public static implicit operator T(Optional<T> optional)
  {
    return optional.Value;
  }

  /// <summary>
  /// Determines whether the specified <see cref="Optional{T}"/> objects are equal.
  /// </summary>
  /// <param name="left"></param>
  /// <param name="right"></param>
  /// <returns></returns>
  public static bool operator ==(Optional<T> left, Optional<T> right)
  {
    return left.Equals(right);
  }

  /// <summary>
  /// Determines whether the specified <see cref="Optional{T}"/> objects are not equal.
  /// </summary>
  /// <param name="left"></param>
  /// <param name="right"></param>
  /// <returns></returns>
  public static bool operator !=(Optional<T> left, Optional<T> right)
  {
    return !left.Equals(right);
  }

  /// <summary>
  /// Returns the value of the current <see cref="Optional{T}"/> object if it has been set; otherwise, returns the specified default value.
  /// </summary>
  /// <param name="defaultValue">The value to be returned if the current <see cref="Optional{T}"/> object has no value.</param>
  /// <returns></returns>
  public T GetValueOr(T defaultValue) => HasValue ? _value : defaultValue;

  public Optional<T> GetValueOrOptional(Optional<T> defaultValue) => HasValue ? this : defaultValue;

  /// <inheritdoc />
  public override string ToString() => HasValue ? _value.ToString() : "Empty";

  /// <inheritdoc />
  public override bool Equals(object obj)
  {
    if (obj is null)
    {
      return !HasValue;
    }
    else if (obj.GetType() == typeof(T))
    {
      return HasValue && _value.Equals(obj);
    }
    else if (obj is Optional<T> opt)
    {
      if (HasValue && opt.HasValue)
      {
        return Equals(opt.Value);
      }
      else
      {
        return !HasValue && !opt.HasValue;
      }
    }

    return false;
  }

  /// <inheritdoc />
  public override int GetHashCode() => HasValue ? _value!.GetHashCode() : 0;

  /// <summary>
  /// Represents an empty <see cref="Optional{T}"/> object.
  /// </summary>
#pragma warning disable CA1000 // Do not declare static members on generic types
  public static Optional<T> Empty => new();
#pragma warning restore CA1000 // Do not declare static members on generic types
}
