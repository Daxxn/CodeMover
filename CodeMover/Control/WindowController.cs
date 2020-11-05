using CodeMover.Exceptions;
using CodeMover.Logic;

using System;
using System.Collections.Generic;
using System.Threading;

namespace CodeMover.Control
{
   public static class WindowController
   {
      #region - Fields & Properties
      private static Dictionary<Command, string> CommandDict { get; } = new Dictionary<Command, string>
        {
            { Command.run, $" - Runs the code copy operation." },
            { Command.src, $" - Sets the source of the files to copy." },
            { Command.dest, $" - Sets the destination of the files." },
            { Command.settings, $" = Sets a setting to a given value. ex: {Command.settings} Exclude.Folders.add=folder" }
        };
      private static int animationFrame { get; set; }
      private static string[] animation { get; } = new string[]
      {
            "|", "/", "-", "\\",
      };
      #endregion

      #region - Methods
      public static void PrintMessage(string message)
      {
         Console.WriteLine(message);
         Console.WriteLine();
      }

      public static void PrintMessage(object message)
      {
         Console.WriteLine(message.ToString());
         Console.WriteLine();
      }

      public static void PrintError(Exception e)
      {
         var originalColor = Console.ForegroundColor;
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine($"ERROR - {e.Message}");
         Console.ForegroundColor = originalColor;
      }

      public static void PrintErrorList(CopyFilesException e)
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

      public static void PrintResult(FileRecord file)
      {
         Console.WriteLine(file.ToString());
      }

      public static void PrintDictionary<K, V>(Dictionary<K, V> dict)
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

      public static void PrintList<T>(IEnumerable<T> list, string title)
      {
         Console.WriteLine($"{title} :");
         foreach (var l in list)
         {
            Console.WriteLine($"   {l}");
         }
      }

      public static void PrintExcludeList(IEnumerable<string> files, IEnumerable<string> folders)
      {
         Console.WriteLine("Exclude:");
         PrintList(files, " Files:");
         PrintList(folders, " Folders:");
      }

      public static void PrintInvalidCommand(string input)
      {
         Console.WriteLine($"Invalid - {(input == "" ? "*Nothing" : input)}");
      }

      public static void PrintCommands()
      {
         foreach (var kv in CommandDict)
         {
            Console.WriteLine($"{kv.Key} {kv.Value}");
         }
      }

      public static void PrintWorkingMessage()
      {
         Thread.Sleep(60);
         Console.WriteLine($"Working {PrintWorkAnimation()}");
         Console.SetCursorPosition(0, Console.CursorTop - 1);

      }

      public static void PrintPaths()
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

      private static string PrintWorkAnimation()
      {
         animationFrame++;
         if (animationFrame >= animation.Length)
         {
            animationFrame = 0;
         }
         return animation[animationFrame];
      }
      #endregion

      #region - Full Properties

      #endregion
   }
}
