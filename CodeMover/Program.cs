using CodeMover.Control;
using CodeMover.Control.Strategies;
using CodeMover.Exceptions;
using CodeMover.Logic;
using CodeMover.Logic.Exclude;
using CodeMover.Logic.Settings;

using System;
using System.Reflection;
using System.Threading;

namespace CodeMover
{
   class Program
   {
      #region Properties
      private static Progress<FileRecord> Progress { get; set; }
      private static FileController FileController { get; } = FileController.Instance;
      public static WindowController WindowController { get; set; } = WindowController.Instance;
      public static SettingsModel Settings { get; private set; }

      public static CancellationTokenSource CancelSource { get; set; } = new CancellationTokenSource();
      #endregion

      static void Main(string[] args)
      {
         try
         {
            Progress = new Progress<FileRecord>();
            Progress.ProgressChanged += ProgressChanged;

            Console.CancelKeyPress += CancelRequestEvent;
            //CancelSource.Token.Register(ExecutionCanceled);

            Settings = SettingsLoader.LoadSettings(ParseArgs(args));
            Excluder.Instance.SetSettings(Settings.Exclude);

            FileController.AddObserver(WindowController);

            var cont = true;

            WindowController.PrintMessage($"Code Mover {GetVersion()}");

            WindowController.PrintExcludeList(Settings.Exclude.Files, Settings.Exclude.Folders);

            IStrategy strategy = new InvalidStrategy();
            while (cont)
            {
               WindowController.PrintPaths();
               //working = true;
               strategy = Interpreter.ExecuteCommand(GetUserInput());

               if (strategy is null)
               {
                  ExecutionCanceled();
                  continue;
               }

               if (strategy is ExitStrategy)
               {
                  cont = false;
                  break;
               }

               try
               {
                  var task = strategy.Run(Progress, CancelSource.Token);
                  WindowController.StartAnimation();
                  task.Wait(CancelSource.Token);
                  WindowController.EndAnimation();
               }
               catch (OperationCanceledException)
               {
                  ExecutionCanceled();
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
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }
         finally
         {
            FileController.RemoveObserver(WindowController);
            Console.WriteLine("\n\nFinished.");
         }
      }

      private static void CancelRequestEvent(object sender, ConsoleCancelEventArgs e)
      {
         e.Cancel = true;
         CancelSource.Cancel();
      }

      private static void ProgressChanged(object sender, FileRecord e)
      {
         WindowController.PrintResult(e);
      }

      private static void ExecutionCanceled()
      {
         Console.WriteLine("Execution Canceled...");
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
