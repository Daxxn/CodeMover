using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMover.Logic
{
   public class FileRecord
   {
      public string Source { get; }
      public string Destination { get; }
      public bool Success { get; set; }
      public Exception Error { get; set; }

      public FileRecord(string source, string dest, bool success, Exception error)
      {
         Source = source;
         Destination = dest;
         Success = success;
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
   }
}
