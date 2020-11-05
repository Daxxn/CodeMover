using CodeMover.Control.Strategies.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class ExecutionTestStrat : IStrategy, IPathResult
   {
      #region Properties
      public Status Status { get; set; }
      public string Results { get; set; }
      #endregion

      #region Methods
      public async Task<Status> Run()
      {
         Status = Status.working;
         return await Task.Run(() =>
         {
            Thread.Sleep(5000);
            return Status.done;
         });
      }
      #endregion
   }
}
