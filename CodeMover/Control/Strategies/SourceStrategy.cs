using CodeMover.Control.Strategies.ResultInterfaces;
using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class SourceStrategy : IStrategy, IArgs, IPathResult
   {
      #region Properties
      private FileController FileController { get; } = FileController.Instance;
      public string Args { get; set; }
      public Status Status { get; set; }
      public string Results { get; set; }
      #endregion

      #region Methods
      public async Task<Status> Run(IProgress<FileRecord> progress, CancellationToken cancelToken)
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
      #endregion
   }
}
