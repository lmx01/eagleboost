// // Author   : Shuo Zhang
// // E-MAIL   : eagleboost@msn.com
// // Creation : 22 9:40 PM

using System;

namespace eagleboost.core.Threading.CancelationTokenTimeout
{
  /// <summary>
  /// CallerOperationTimeoutException
  /// </summary>
  public class CallerOperationTimeoutException : Exception
  {
    public CallerOperationTimeoutException(string msg, Exception e) : base(msg, e)
    {
    }

    public CallerOperationTimeoutException(Exception e) : this(e.Message, e)
    {
    }
  }

  /// <summary>
  /// OperationCanceledByCallerException
  /// </summary>
  public class OperationCanceledByCallerException : Exception
  {
    public OperationCanceledByCallerException(string msg, Exception e) : base(msg, e)
    {
    }

    public OperationCanceledByCallerException(Exception e) : this(e.Message, e)
    {
    }
  }
}