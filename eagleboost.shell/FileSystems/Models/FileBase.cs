// Author : Shuo Zhang
// E-MAIL : eagleboost@msn.com
// Creation :2018-08-31 12:45 AM

namespace eagleboost.shell.FileSystems.Models
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using eagleboost.core.ComponentModel;
  using eagleboost.core.Types;
  using eagleboost.shell.FileSystems.Contracts;

  /// <summary>
  /// FileBase
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="TFolder"></typeparam>
  public abstract class FileBase<T, TFolder> : IFile, IComparer, IComparer<T>
    where T : FileBase<T, TFolder>
    where TFolder : class, IFolder
  {
    #region ctors
    protected FileBase(TFolder parent)
    {
      Parent = parent;
    }
    #endregion ctors

    #region Public Properties
    public abstract string Id { get; }

    public abstract string Name { get; }

    string IDisplayItem.DisplayName
    {
      get { return Name; }
    }

    public abstract string Type { get; }

    public abstract long? Size { get; }

    public FileSize? FileSize
    {
      get
      {
        var size = Size;
        if (size.HasValue)
        {
          return new FileSize(size.Value);
        }

        return null;
      }
    }

    IFolder IFile.Parent
    {
      get { return Parent; }
    }

    public TFolder Parent { get; private set; }
    #endregion Public Properties

    #region IComparer
    public int Compare(T x, T y)
    {
      return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }

    int IComparer.Compare(object x, object y)
    {
      return Compare((T)x, (T)y);
    }
    #endregion IComparer

    #region Overrides
    public override string ToString()
    {
      return Name;
    }
    #endregion Overrides
  }
}