using CodeMover.Control.Strategies.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class ExitStrategy : IStrategy, IPathResult
   {
      #region Properties
      public Status Status { get; set; }
      public string Results { get; set; }
      #endregion

      #region Methods
      public async Task<Status> Run()
      {
         Results = "";
         return await Task.Run(() => Status.done);
      }
      #endregion
   }
}
