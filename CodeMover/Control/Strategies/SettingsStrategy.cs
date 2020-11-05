using CodeMover.Control.Strategies.ResultInterfaces;
using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class SettingsStrategy : IStrategy, IArgs, IPathResult
   {
      #region Properties
      public string Args { get; set; }
      public Status Status { get; set; }
      public string Results { get; set; }
      #endregion

      #region Methods
      public async Task<Status> Run()
      {
         try
         {
            Status = Status.working;
            return await Task.Run(() =>
            {
               try
               {
                  var settings = Program.Settings;
                  var props = settings.GetType().GetProperties();
                  string[] split = Args.Split('=', StringSplitOptions.RemoveEmptyEntries);
                  if (split.Length == 2)
                  {
                     string[] splitAccessor = split[0].Split('.', StringSplitOptions.RemoveEmptyEntries);
                     if (splitAccessor.Length == 1)
                     {
                        var pass = false;
                        var newDir = split[1];
                        if (splitAccessor[0].ToLower() == Accessor.destination.ToString())
                        {
                           (pass, newDir) = FileController.CreateDir(split[1]);
                        }
                        else
                        {
                           pass = FileController.DirExists(newDir);
                        }
                        if (pass)
                        {
                           var prop = props.First((p) => p.Name.ToLower() == splitAccessor[0].ToLower());
                           if (prop.CanWrite)
                           {
                              prop.SetValue(settings, newDir);
                           }
                        }
                     }
                     else if (splitAccessor.Length == 3)
                     {
                        var prop = props
                           .First((p) =>
                           {
                              return p.Name.ToLower() == splitAccessor[0].ToLower();
                           }).PropertyType.GetProperties()
                              .First((innerP) =>
                              {
                                 return innerP.Name.ToLower() == splitAccessor[1].ToLower();
                              });
                        if (prop.CanWrite)
                        {
                           var success = Enum.TryParse(typeof(Action), splitAccessor[2], out var action);
                           if (success)
                           {
                              prop.SetValue(
                                 settings.Exclude,
                                 AddToArray(
                                    prop.GetValue(settings.Exclude) as string[],
                                    split[1]));
                           }
                        }
                     }
                     else
                     {
                        throw new Exception("No match found.");
                     }
                  }
                  else
                  {
                     throw new Exception("Too many arguments.");
                  }
                  return Status.done;
               }
               catch (Exception)
               {
                  throw;
               }
            });
         }
         catch (Exception)
         {
            throw;
         }
      }

      private string[] AddToArray(string[] array, string value)
      {
         var newArray = new string[array.Length + 1];
         for (int i = 0; i < array.Length; i++)
         {
            newArray[i] = array[i];
         }
         newArray[array.Length] = value;
         return newArray;
      }
      #endregion
   }
}
