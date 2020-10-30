using CodeMover.Logic.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeMover.Logic.Exclude
{
   public class Excluder
   {
      #region - Fields & Properties
      public static Excluder Instance { get; } = new Excluder();

      public ExcludeSetting Settings { get; private set; }
      private bool _isSet = false;
      #endregion

      #region - Constructors
      private Excluder() { }
      #endregion

      #region - Methods

      #region File Exclusion Methods
      public string[] Run(string[] filePaths, FileType type = FileType.All)
      {
         switch (type)
         {
            case FileType.File:
               return Filter(filePaths, Settings.Files);
            case FileType.Directory:
               return Filter(filePaths, Settings.Folders);
            case FileType.All:
               var filteredDirs = Filter(filePaths, Settings.Folders);
               return Filter(filteredDirs, Settings.Files);
            default:
               throw new Exception($"No type option found - {type}");
         }
      }

      private string[] Filter(string[] files, string[] criteria)
      {
         return files.Where((file) => !criteria.Any((c) => file.Contains(c))).ToArray();
      }
      #endregion

      public void SetSettings(ExcludeSetting settings)
      {
         if (!_isSet)
         {
            Settings = settings;
         }
         _isSet = true;
      }
      #endregion

      #region - Full Properties
      #endregion
   }
}
