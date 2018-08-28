// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-27 6:17 PM

namespace eagleboost.core.Types
{
  using System;
  using eagleboost.core.Extensions;

  /// <summary>
  /// FileSize
  /// </summary>
  public struct FileSize : IComparable, IComparable<FileSize>, IEquatable<FileSize>
  {
    #region Declarations
    private readonly long _byteSize;
    private readonly string _displaySize;
    private readonly string _displaySizeUnit;
    #endregion Declarations

    #region ctors
    public FileSize(long byteSize) : this()
    {
      _byteSize = byteSize;
      ReadableSize = byteSize.ToReadableSize(out _displaySize, out _displaySizeUnit);
    }
    #endregion ctors

    #region Public Properties
    public long ByteSize
    {
      get { return _byteSize; }
    }

    public string DisplaySize
    {
      get { return _displaySize; }
    }

    public string DisplaySizeUnit
    {
      get { return _displaySizeUnit; }
    }

    public string ReadableSize { get; private set; }
    #endregion Public Properties

    #region IComparable
    public int CompareTo(object obj)
    {
      return CompareTo((FileSize) obj);
    }

    public int CompareTo(FileSize other)
    {
      return ByteSize.CompareTo(other.ByteSize);
    }
    #endregion IComparable

    #region IEquatable
    public bool Equals(FileSize other)
    {
      return ByteSize == other.ByteSize;
    }
    #endregion IEquatable

    #region Overrides
    public override int GetHashCode()
    {
      return ByteSize.GetHashCode();
    }

    public override string ToString()
    {
      return ReadableSize;
    }
    #endregion Overrides
  }
}