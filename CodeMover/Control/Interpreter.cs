using CodeMover.Control.Strategies;
using CodeMover.Control.Strategies.ResultInterfaces;
using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMover.Control
{

   public static class Interpreter
   {
      #region - Fields & Properties
      #endregion

      #region - Methods
      public static IStrategy ExecuteCommand(string input)
      {
         var cmd = Parse(input);
         switch (cmd.Item1)
         {
            case Command.exit:
               return new ExitStrategy
               {
                  Results = "",
               };
            case Command.run:
               return new ExecuteStrategy();
            case Command.src:
               return new SourceStrategy
               {
                  Args = cmd.Item2,
               };
            case Command.dest:
               return new DestinationStrategy
               {
                  Args = cmd.Item2,
               };
            case Command.settings:
               return new SettingsStrategy
               {
                  Args = cmd.Item2,
               };
            case Command.invalid:
               return new InvalidStrategy
               {
                  Args = input,
               };
            default:
               throw new ArgumentException("Command Error. Could not find strategy.");
         }
      }

      private static (Command, string) Parse(string input)
      {
         if (String.IsNullOrEmpty(input))
         {
            return (Command.invalid, null);
         }
         var split = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
         if (split.Length == 1)
         {
            bool success = Enum.TryParse(split[0].ToLower(), out Command cmd);
            return (cmd, null);
         }
         else if (split.Length == 2)
         {
            bool success = Enum.TryParse(split[0].ToLower(), out Command cmd);
            return (cmd, split[1]);

         }
         return (Command.invalid, null);
      }

      public static IStrategy ExecutionTest()
      {
         return new ExecutionTestStrat();
      }
      #endregion

      #region - Full Properties
      #endregion
   }
}
