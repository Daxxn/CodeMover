using CodeMover.Exceptions;
using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class ExecuteStrategy : IStrategy, IFileResults
   {
      #region Properties
      private FileController FileController { get; } = FileController.Instance;
      public Status Status { get; set; }
      public List<FileRecord> Results { get; set; }
      #endregion

      #region Methods
      public async Task<Status> Run(IProgress<FileRecord> progress, CancellationToken cancelToken)
      {
         Status = Status.working;
         try
         {
            Results = await FileController.NewMoveDirAsync(progress, cancelToken);
         }
         catch (Exception)
         {
            throw;
         }
         return Status.done;
      }

      public Status Run(bool blah)
      {
         Results = FileController.MoveDir().ToList();
         return Status.done;
      }
      #endregion
   }
}
