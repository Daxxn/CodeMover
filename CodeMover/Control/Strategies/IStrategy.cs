using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public interface IStrategy : IStrategyResults
   {
      Status Status { get; set; }
      Task<Status> Run(IProgress<FileRecord> progress, CancellationToken cancelToken);
   }
}
