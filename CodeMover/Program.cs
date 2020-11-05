using CodeMover.Control;
using CodeMover.Control.Strategies;
using CodeMover.Exceptions;
using CodeMover.Logic;
using CodeMover.Logic.Exclude;
using CodeMover.Logic.Settings;

using System;
using System.Reflection;

namespace CodeMover
{
   class Program
   {
      public static SettingsModel Settings { get; private set; }
      static void Main(string[] args)
      {
         try
         {
            Settings = SettingsLoader.LoadSettings(ParseArgs(args));
            Excluder.Instance.SetSettings(Settings.Exclude);

            var cont = true;
            var working = false;

            WindowController.PrintMessage($"Code Mover {GetVersion()}");

            WindowController.PrintExcludeList(Settings.Exclude.Files, Settings.Exclude.Folders);

            IStrategy strategy = new InvalidStrategy();
            while (cont)
            {
               if (!working)
               {
                  WindowController.PrintPaths();
                  working = true;
                  strategy = Interpreter.ExecuteCommand(GetUserInput());

                  if (strategy is ExitStrategy)
                  {
                     cont = false;
                     break;
                  }

                  try
                  {
                     var task = strategy.Run();
                     task.ContinueWith((task) =>
                     {
                        working = false;
                     });
                  }
                  catch (CopyFilesException e)
                  {
                     WindowController.PrintErrorList(e);
                  }
                  catch (Exception e)
                  {
                     WindowController.PrintError(e);
                  }
               }
               else
               {
                  WindowController.PrintWorkingMessage();
                  if (strategy.Status == Status.done)
                  {
                     if (strategy is IFileResults)
                     {
                        WindowController.PrintMessage("Copy complete.");
                     }
                     else
                     {
                        if (strategy.Status == Status.done)
                        {
                           if (strategy is ExitStrategy)
                           {
                              cont = false;
                              break;
                           }
                           else if (strategy is InvalidStrategy)
                           {
                              WindowController.PrintMessage("Invalid Command.");
                              WindowController.PrintList(Enum.GetNames(typeof(Command)), "Commands");
                           }
                        }
                     }
                  }
               }
            }
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }
         finally
         {
            Console.WriteLine("\n\nFinished.");
         }
      }

      public static string GetUserInput()
      {
         Console.Write("> ");
         return Console.ReadLine();
      }

      public static string ParseArgs(string[] args)
      {
         string def = @"B:\Code\CodeMoverSettings.json";
         if (args.Length > 0)
         {
            if (FileController.MatchExt(args[0], ".json"))
            {
               return args[0];
            }
         }
         return def;
      }

      private static string GetVersion()
      {
         var assembly = Assembly.GetExecutingAssembly();
         var name = assembly.GetName();
         return name.Version.ToString();
      }
   }
}
