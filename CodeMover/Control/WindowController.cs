using CodeMover.Control.Observers;
using CodeMover.Exceptions;
using CodeMover.Logic;

using System;
using System.Collections.Generic;
using System.Threading;

namespace CodeMover.Control
{
   public class WindowController : IObserver
   {
      #region - Fields & Properties
      private static WindowController _instance;
      private Dictionary<Command, string> CommandDict { get; } = new Dictionary<Command, string>
        {
            { Command.run, $" - Runs the code copy operation." },
            { Command.src, $" - Sets the source of the files to copy." },
            { Command.dest, $" - Sets the destination of the files." },
            { Command.settings, $" = Sets a setting to a given value. ex: {Command.settings} Exclude.Folders.add=folder" }
        };
      private int animationFrame { get; set; }
      private string[] animation { get; } = new string[]
      {
            "|", "/", "-", "\\",
      };
      public SimpleAnimation Animation { get; set; } = new SimpleAnimation();
      #endregion

      #region Constructors
      private WindowController() { }
      #endregion

      #region - Methods
      public void Update(FileRecord completedFile)
      {
         PrintResult(completedFile);
      }

      public void PrintMessage(string message)
      {
         Console.WriteLine(message);
         Console.WriteLine();
      }

      public void PrintMessage(object message)
      {
         Console.WriteLine(message.ToString());
         Console.WriteLine();
      }

      public void PrintError(Exception e)
      {
         var originalColor = Console.ForegroundColor;
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine($"ERROR - {e.Message}");
         Console.ForegroundColor = originalColor;
      }

      public void PrintErrorList(CopyFilesException e)
      {
         var originalColor = Console.ForegroundColor;
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine($"ERROR - {e.Message}");
         foreach (var error in e.Exceptions)
         {
            Console.WriteLine($"   - {error.Message}");
         }
         Console.ForegroundColor = originalColor;
      }

      public void PrintResult(FileRecord file)
      {
         Console.WriteLine(file.ToString());
      }

      public void PrintDictionary<K, V>(Dictionary<K, V> dict)
      {
         foreach (var kv in dict)
         {
            Console.CursorLeft = 0;
            Console.Write(kv.Key);
            Console.CursorLeft = 10;
            Console.Write($" : {kv.Value}");
            Console.WriteLine();
         }
      }

      public void PrintList<T>(IEnumerable<T> list, string title)
      {
         Console.WriteLine($"{title} :");
         foreach (var l in list)
         {
            Console.WriteLine($"   {l}");
         }
      }

      public void PrintExcludeList(IEnumerable<string> files, IEnumerable<string> folders)
      {
         Console.WriteLine("Exclude:");
         PrintList(files, " Files:");
         PrintList(folders, " Folders:");
      }

      public void PrintInvalidCommand(string input)
      {
         Console.WriteLine($"Invalid - {(input == "" ? "*Nothing" : input)}");
      }

      public void PrintCommands()
      {
         foreach (var kv in CommandDict)
         {
            Console.WriteLine($"{kv.Key} {kv.Value}");
         }
      }

      public void PrintPaths()
      {
         Console.WriteLine();
         var originalColor = Console.BackgroundColor;
         Console.BackgroundColor = ConsoleColor.Blue;
         var src = "Current Source:";
         Console.Write($"{src}        ");
         Console.CursorLeft = src.Length + 2;
         Console.Write(Program.Settings.Source);
         Console.CursorTop++;
         Console.CursorLeft = 0;
         Console.Write($"Current Dest:         ");
         Console.CursorLeft = src.Length + 2;
         Console.WriteLine(Program.Settings.Destination);
         Console.BackgroundColor = originalColor;
         Console.WriteLine();
      }

      public void StartAnimation()
      {
         Animation.Start();
      }

      public void EndAnimation()
      {
         Animation.End();
      }
      #endregion

      #region - Full Properties
      public static WindowController Instance
      {
         get
         {
            if (_instance is null)
            {
               _instance = new WindowController();
            }
            return _instance;
         }
      }
      #endregion
   }
}
