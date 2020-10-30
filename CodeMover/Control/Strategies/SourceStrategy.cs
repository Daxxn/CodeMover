using CodeMover.Control.Strategies.ResultInterfaces;
using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class SourceStrategy : IStrategy, IArgs, IPathResult
   {
      public string Args { get; set; }
      public Status Status { get; set; }
      public string Results { get; set; }
      //object IStrategyResults.Results { get => Results; set => Results = value as string; }

      public async Task<Status> Run()
      {
         Status = Status.working;
         return await Task.Run(() =>
         {
            if (FileController.DirExists(Args))
            {
               Program.Settings.Source = Args;
               Results = Args;
            }
            return Status.done;
         });
      }
   }
}
