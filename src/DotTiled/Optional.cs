using System;

namespace DotTiled;

/// <summary>
/// Represents an empty value.
/// </summary>
public readonly struct OptionalEmpty;

/// <summary>
/// Represents an empty <see cref="Optional{T}"/> object.
/// </summary>
public static class Optional
{
  /// <summary>
  /// Represents an empty <see cref="Optional{T}"/> object.
  /// </summary>
  public static readonly OptionalEmpty Empty = default;
}

/// <summary>
/// Represents a value that may or may not be present.
/// </summary>
/// <typeparam name="T">The type of the optionally present value.</typeparam>
public readonly struct Optional<T>
{
#pragma warning disable IDE0032 // Use auto property
  private readonly bool _hasValue;
  private readonly T _value;

  /// <summary>
  /// Returns <see langword="true"/> if the <see cref="Value"/> will return a meaningful value.
  /// </summary>
  /// <returns></returns>
  public bool HasValue => _hasValue;
#pragma warning restore IDE0032 // Use auto property

  /// <summary>
  /// Constructs an <see cref="Optional{T}"/> with a meaningful value.
  /// </summary>
  /// <param name="value"></param>
  public Optional(T value)
  {
    _hasValue = true;
    _value = value;
  }

  /// <summary>
  /// Gets the value of the current object.  Not meaningful unless <see cref="HasValue"/> returns <see langword="true"/>.
  /// </summary>
  /// <remarks>
  /// <para>Unlike <see cref="Nullable{T}.Value"/>, this property does not throw an exception when
  /// <see cref="HasValue"/> is <see langword="false"/>.</para>
  /// </remarks>
  /// <returns>
  /// <para>The value if <see cref="HasValue"/> is <see langword="true"/>; otherwise, the default value for type
  /// <typeparamref name="T"/>.</para>
  /// </returns>
  public T Value => HasValue ? _value : throw new InvalidOperationException("Value is not set");

  /// <summary>
  /// Creates a new object initialized to a meaningful value.
  /// </summary>
  /// <param name="value"></param>
  public static implicit operator Optional<T>(T value)
  {
    if (value is null)
    {
      return default;
    }

    return new Optional<T>(value);
  }

  /// <summary>
  /// Creates a new object initialized to an empty value.
  /// </summary>
  /// <param name="_"></param>
  public static implicit operator Optional<T>(OptionalEmpty _)
  {
    return default;
  }

  /// <summary>
  /// Returns a string representation of this object.
  /// </summary>
  public override string ToString()
  {
    // Note: For nullable types, it's possible to have _hasValue true and _value null.
    return _hasValue
        ? _value?.ToString() ?? "null"
        : "Empty";
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
  /// Returns the value of the current <see cref="Optional{T}"/> object if it has been set; otherwise, returns the specified default value.
  /// </summary>
  /// <param name="defaultValue">The value to be returned if the current <see cref="Optional{T}"/> object has no value.</param>
  /// <returns></returns>
  public T GetValueOr(T defaultValue) => HasValue ? _value : defaultValue;

  /// <summary>
  /// Returns the current <see cref="Optional{T}"/> object if it has a value; otherwise, returns the specified default value.
  /// </summary>
  /// <param name="defaultValue">The <see cref="Optional{T}"/> object to be returned if the current <see cref="Optional{T}"/> object has no value.</param>
  /// <returns></returns>
  public Optional<T> GetValueOrOptional(Optional<T> defaultValue) => HasValue ? this : defaultValue;

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
  /// Determines whether the specified <see cref="Optional{T}"/> objects are not equal.
  /// </summary>
  /// <param name="left"></param>
  /// <param name="right"></param>
  /// <returns></returns>
  public static bool operator !=(Optional<T> left, Optional<T> right)
  {
    return !left.Equals(right);
  }
}
