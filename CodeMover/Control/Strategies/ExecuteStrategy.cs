using CodeMover.Exceptions;
using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class ExecuteStrategy : IStrategy, IFileResults
   {
      #region Properties
      public Status Status { get; set; }
      public List<FileRecord> Results { get; set; }
      #endregion

      #region Methods
      public async Task<Status> Run()
      {
         Status = Status.working;
         try
         {
            Results = await FileController.NewMoveDirAsync();
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
