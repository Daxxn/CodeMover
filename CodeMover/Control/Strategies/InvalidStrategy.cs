using CodeMover.Control.Strategies.ResultInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class InvalidStrategy : IStrategy, IPathResult
   {
      #region Properties
      private WindowController WindowController { get; set; } = WindowController.Instance;
      public string Args { get; set; }
      public Status Status { get; set; }
      public string Results { get; set; }
      #endregion

      #region Methods
      public async Task<Status> Run()
      {
         return await Task.Run(() =>
         {
            WindowController.PrintInvalidCommand(Args);
            WindowController.PrintCommands();
            return Status.done;
         });
      }
      #endregion
   }
}
