using CodeMover.Control.Strategies.ResultInterfaces;
using CodeMover.Logic;
using System;
using System.Threading;
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
      public async Task<Status> Run(IProgress<FileRecord> progress, CancellationToken cancelToken)
      {
         Results = "";
         return await Task.Run(() => Status.done);
      }
      #endregion
   }
}
