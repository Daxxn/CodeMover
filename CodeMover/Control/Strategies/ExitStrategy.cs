using CodeMover.Control.Strategies.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class ExitStrategy : IStrategy, IPathResult
   {
      public Status Status { get; set; }
      public string Results { get; set; }
      //object IStrategyResults.Results { get => Results; set => Results = value as string; }

      public async Task<Status> Run()
      {
         Results = "";
         return await Task.Run(() => Status.done);
      }
   }
}
