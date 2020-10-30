using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CodeMover.Exceptions
{
   public class CopyFilesException : Exception
   {
      public List<Exception> Exceptions { get; } = new List<Exception>();
      public CopyFilesException()
      {
      }
      public CopyFilesException(List<Exception> excs) => Exceptions = excs;
      public CopyFilesException(string message) : base(message) { }
      public CopyFilesException(string message, List<Exception> excs) : base(message) => Exceptions = excs;
      public CopyFilesException(string message, Exception innerException) : base(message, innerException) { }
      public CopyFilesException(
          string message,
          List<Exception> excs,
          Exception innerException
      ) : base(message, innerException) => Exceptions = excs;
   }
}
