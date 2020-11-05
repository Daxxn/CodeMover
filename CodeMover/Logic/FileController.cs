using CodeMover.Control;
using CodeMover.Control.Observers;
using CodeMover.Exceptions;
using CodeMover.Logic.Exclude;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMover.Logic
{
   public class FileController : FileObservable
   {
      #region - Fields & Properties
      private static FileController _instance;
      #endregion

      #region Constructors
      private FileController() { }
      #endregion

      #region - Methods
      public IEnumerable<FileRecord> MoveDir()
      {
         try
         {
            var source = Program.Settings.Source;
            var destination = Program.Settings.Destination;

            List<FileRecord> movedFiles = new List<FileRecord>();
            List<Exception> errors = new List<Exception>();

            string[] unfilteredFiles = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
            string[] files = Excluder.Instance.Run(unfilteredFiles);

            CreateDirs(source, destination);

            if (files.Length > 0)
            {
               foreach (var file in files)
               {
                  movedFiles.Add(CopyFile(source, file, destination));
               }
            }

            if (errors.Count > 0)
            {
               throw files.Length == errors.Count ?
                   new CopyFilesException("Some files failed to copy.", errors) :
                   new Exception("All files failed to copy.");
            }

            return movedFiles;
         }
         catch (Exception e)
         {
            throw e;
         }
      }

      public async Task<List<FileRecord>> MoveDirAsyncOld(string source, string destination)
      {
         try
         {
            List<Task<FileRecord>> tasks = new List<Task<FileRecord>>();
            string[] files = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
               tasks.Add(Task.Run(() =>
               {
                  return CopyFile(source, file, destination);
               }));
            }

            var results = await Task.WhenAll(tasks);
            return results.ToList();
         }
         catch (Exception)
         {
            throw;
         }
      }

      public async Task<List<FileRecord>> MoveDirAsync()
      {
         try
         {
            var source = Program.Settings.Source;
            var destination = Program.Settings.Destination;

            List<Exception> errors = new List<Exception>();
            List<FileRecord> fileRecords = new List<FileRecord>();
            List<Task<FileRecord>> tasks = new List<Task<FileRecord>>();

            string[] Unfilteredfiles = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);

            string[] files = Excluder.Instance.Run(Unfilteredfiles);

            await Task.Run(() => CreateDirs(source, destination));

            foreach (var file in files)
            {
               try
               {
                  tasks.Add(Task.Run(() =>
                  {
                     return CopyFile(source, file, destination);
                  }));
               }
               catch (Exception e)
               {
                  errors.Add(e);
               }
            }

            if (errors.Count > 0)
            {
               throw new CopyFilesException(errors);
            }

            return (await Task.WhenAll(tasks)).ToList();
         }
         catch (Exception)
         {
            throw;
         }
      }

      /// <summary>
      /// Moves a full directory, recursively and async - Final Version
      /// </summary>
      /// <returns>
      /// An asynchronous Task list of FileRecords.
      /// The FileRecord list is just for display purposes with no functionality.
      /// </returns>
      public async Task<List<FileRecord>> NewMoveDirAsync()
      {
         try
         {
            var source = Program.Settings.Source;
            var destination = Program.Settings.Destination;

            List<Exception> errors = new List<Exception>();
            List<FileRecord> fileRecords = new List<FileRecord>();
            List<Task<FileRecord>> tasks = new List<Task<FileRecord>>();

            string[] Unfilteredfiles = Directory.GetFiles(source, "*.*", SearchOption.AllDirectories);

            string[] files = Excluder.Instance.Run(Unfilteredfiles);

            CreateDirs(source, destination);

            foreach (var file in files)
            {
               try
               {
                  tasks.Add(Task.Run(() =>
                  {
                     using (Copy copy = new Copy(BuildFilePath(source, file, destination)))
                     {
                        var copiedFile = copy.CopyFile();
                        Notify(copiedFile);
                        return copiedFile;
                     }
                  }));
               }
               catch (Exception e)
               {
                  errors.Add(e);
               }
            }

            if (errors.Count > 0)
            {
               throw new CopyFilesException(errors);
            }

            return (await Task.WhenAll(tasks)).ToList();
         }
         catch (Exception)
         {
            throw;
         }
      }

      public bool FileExists(string path)
      {
         return File.Exists(path);
      }

      public bool DirExists(string path)
      {
         return Directory.Exists(path);
      }

      public bool MatchExt(string path, string ext)
      {
         return FileExists(path) ? Path.GetExtension(path) == ext : false;
      }

      #region Private Methods
      private string[] GetFilePaths(string dirPath)
      {
         throw new NotImplementedException(nameof(GetFilePaths));
      }
      #endregion

      private FileRecord CopyFile(string src, string file, string dest)
      {
         try
         {
            var relPath = Path.GetRelativePath(src, file);
            var newPath = Path.Combine(dest, relPath);

            Console.WriteLine(relPath);
            Console.WriteLine(newPath);

            File.Copy(file, newPath, true);

            var completedFile = new FileRecord(file, newPath, null);
            //WindowController.PrintResult(completedFile);
            return completedFile;
         }
         catch (Exception e)
         {
            return new FileRecord(src, dest, e);
         }
      }

      /// <summary>
      /// Takes the file and returns a new FileRecord with the files new path.
      /// </summary>
      /// <param name="src">The source directory</param>
      /// <param name="file">The full source file path.</param>
      /// <param name="dest">The destination directory.</param>
      /// <returns>A new FileRecord with the source, destination and new file paths.</returns>
      private FileRecord BuildFilePath(string src, string file, string dest)
      {
         var relPath = Path.GetRelativePath(src, file);
         var newPath = Path.Combine(dest, relPath);

         return new FileRecord(src, dest, file, newPath, null);
      }

      public (bool, string) CreateDir(string input)
      {
         try
         {
            var info = Directory.CreateDirectory(input);

            return (true, info.FullName);
         }
         catch (Exception)
         {
            return (false, null);
         }
      }

      /// <summary>
      /// Recreates the source directory structure in the destination directory.
      /// </summary>
      /// <param name="source">Source directory</param>
      /// <param name="dest">Destination directory</param>
      public void CreateDirs(string source, string dest)
      {
         string[] unfilteredDirs = Directory.GetDirectories(source, "", SearchOption.AllDirectories);
         string[] dirs = Excluder.Instance.Run(unfilteredDirs, FileType.Directory);

         foreach (var dir in dirs)
         {
            var relPath = Path.GetRelativePath(source, dir);
            var path = Path.Combine(dest, relPath);

            if (!Directory.Exists(path))
            {
               Directory.CreateDirectory(path);
            }
         }
      }
      #endregion

      #region - Full Properties
      public static FileController Instance
      {
         get
         {
            if (_instance is null)
            {
               _instance = new FileController();
            }
            return _instance;
         }
      }
      #endregion
   }
}
