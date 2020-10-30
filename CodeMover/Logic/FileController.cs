using CodeMover.Control;
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
   public static class FileController
   {
      #region - Fields & Properties

      #endregion

      #region - Methods
      public static IEnumerable<FileRecord> MoveDir()
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
                  //try
                  //{
                  //    var relPath = Path.GetRelativePath(source, file);
                  //    var newPath = Path.Combine(destination, relPath);

                  //    File.Copy(file, Path.Combine(destination, newPath));

                  //    movedFiles.Add(new FileRecord(file, newPath, true, null));
                  //}
                  //catch (Exception e)
                  //{
                  //    errors.Add(e);
                  //}
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

      public static async Task<List<FileRecord>> MoveDirAsyncOld(string source, string destination)
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

      public static async Task<List<FileRecord>> MoveDirAsync()
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

            //return fileRecords;
         }
         catch (Exception)
         {
            throw;
         }
      }

      public static bool FileExists(string path)
      {
         return File.Exists(path);
      }

      public static bool DirExists(string path)
      {
         return Directory.Exists(path);
      }

      public static bool MatchExt(string path, string ext)
      {
         return FileExists(path) ? Path.GetExtension(path) == ext : false;
      }

      #region Private Methods
      private static string[] GetFilePaths(string dirPath)
      {
         throw new NotImplementedException(nameof(GetFilePaths));
      }
      #endregion

      private static FileRecord CopyFile(string src, string file, string dest)
      {
         try
         {
            var relPath = Path.GetRelativePath(src, file);
            var newPath = Path.Combine(dest, relPath);

            Console.WriteLine(relPath);
            Console.WriteLine(newPath);

            //CreateDirs(relPath, dest);

            File.Copy(file, newPath, true);

            var completedFile = new FileRecord(file, newPath, true, null);
            WindowController.PrintResult(completedFile);
            return completedFile;
         }
         catch (Exception e)
         {
            return new FileRecord(src, dest, false, e);
         }
      }

      public static (bool, string) CreateDir(string input)
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

      public static void CreateDirs(string source, string dest)
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

      #endregion
   }
}
