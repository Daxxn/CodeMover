using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMover.Logic
{
   public class FileRecord
   {
      public string Source { get; }
      public string Destination { get; }
      public string SourceFilePath { get; set; }
      public string DestFilePath { get; set; }
      public Exception Error { get; set; }
      public bool _success;

      public FileRecord(string source, string dest, Exception error)
      {
         Source = source;
         Destination = dest;
         Error = error;
      }
      public FileRecord(string source, string dest, string sourceFile, Exception error)
      {
         Source = source;
         Destination = dest;
         SourceFilePath = sourceFile;
         Error = error;
      }
      public FileRecord(string source, string dest, string sourceFile, string destFile, Exception error)
      {
         Source = source;
         Destination = dest;
         SourceFilePath = sourceFile;
         DestFilePath = destFile;
         Error = error;
      }

      public override string ToString()
      {
         StringBuilder b = new StringBuilder();
         b.AppendLine($"src: {Source}");
         b.AppendLine($"Dest: {Destination}");
         b.AppendLine($"Pass: {Success}");
         if (Error != null)
         {
            b.AppendLine($"Error: {Error}");
         }
         return b.ToString();
      }

      public bool Success
      {
         get => Error is null ? true : false;
      }
   }
}
