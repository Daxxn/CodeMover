using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CodeMover.Logic
{
   public class Copy : IAsyncDisposable
   {
      #region - Fields & Properties
      public FileRecord File { get; set; }
      private StreamWriter Writer { get; set; }
      private StreamReader Reader { get; set; }
      #endregion

      #region - Constructors
      public Copy() { }
      #endregion

      #region - Methods
      public virtual async Task<FileRecord> CopyFileAsyncNoGC()
      {
         var fileData = await Task.Run(() =>
         {
            using (Reader = new StreamReader(File.Source))
            {
               return Reader.ReadToEnd();
            }
         });
         return await Task.Run(() =>
         {
            using (Writer = new StreamWriter(File.Destination))
            {
               try
               {
                  Writer.Write(fileData);
               }
               catch (Exception e)
               {
                  File.Error = e;
               }
            }
            return File;
         });
      }

      public FileRecord CopyFile()
      {
         Reader = new StreamReader(File.SourceFilePath);
         var fileData = Reader.ReadToEnd();
         Reader.Close();
         Writer = new StreamWriter(File.DestFilePath);
         Writer.Write(fileData);
         Writer.Flush();
         Writer.Close();
         return File;
      }

      public virtual async Task<FileRecord> CopyFileAsyncNoUsing()
      {
         try
         {
            Reader = new StreamReader(File.DestFilePath);
            Writer = new StreamWriter(File.Destination);
            System.IO.File.Create(File.DestFilePath);
            var fileData = await Reader.ReadToEndAsync();
            await Writer.WriteAsync(fileData);
         }
         catch (Exception e)
         {
            File.Error = e;
         }
         return File;
      }

      public ValueTask DisposeAsync()
      {
         throw new NotImplementedException(nameof(DisposeAsync));
      }

      private ValueTask DisposeAsyncCore()
      {
         throw new NotImplementedException(nameof(DisposeAsyncCore));
      }
      #endregion

      #region - Full Properties

      #endregion
   }
}
