using CodeMover.Control.Strategies.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class InvalidStrategy : IStrategy, IPathResult
   {
      public string Args { get; set; }
      public Status Status { get; set; }
      public string Results { get; set; }
      //object IStrategyResults.Results { get => Results; set => Results = value as string; }

      public async Task<Status> Run()
      {
         return await Task.Run(() =>
         {
            WindowController.PrintInvalidCommand(Args);
            WindowController.PrintCommands();
            return Status.done;
         });
      }
   }
}
