using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CodeMover.Logic.Settings
{
   public static class SettingsLoader
   {
      #region - Fields & Properties

      #endregion

      #region - Methods
      public static SettingsModel LoadSettings(string path)
      {
         try
         {
            using (StreamReader reader = new StreamReader(path))
            {
               return JsonConvert.DeserializeObject<SettingsModel>(reader.ReadToEnd());
            }
         }
         catch (Exception)
         {
            throw;
         }
      }

      public static void SaveSettings(string path, SettingsModel settings)
      {
         try
         {
            using (StreamWriter writer = new StreamWriter(path))
            {
               writer.Write(JsonConvert.SerializeObject(settings, Formatting.Indented));
            }
         }
         catch (Exception)
         {
            throw;
         }
      }
      #endregion

      #region - Full Properties

      #endregion
   }
}
