using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeMover.Logic
{
   public class Copy2 : IDisposable
   {
      #region - Fields & Properties
      public FileInfo SourceFile { get; set; }
      public FileRecord Record { get; set; }
      #endregion

      #region - Constructors
      public Copy2(FileRecord record)
      {
         Record = record;
      }
      #endregion

      #region - Methods
      public FileRecord CopyFile()
      {
         try
         {
            SourceFile = new FileInfo(Record.SourceFilePath);
            _ = SourceFile.CopyTo(Record.DestFilePath, true);
            return Record;
         }
         catch (Exception)
         {
            throw;
         }
      }

      public void Dispose()
      {
         Record = null;
         SourceFile = null;
      }
      #endregion

      #region - Full Properties

      #endregion
   }
}
