using CodeMover.Control.Strategies.ResultInterfaces;
using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeMover.Control.Strategies
{
   public class ExecutionTestStrat : IStrategy, IPathResult, IArgs
   {
      #region Properties
      private static readonly Random Random = new Random();
      public Status Status { get; set; }
      public string Results { get; set; }
      public string Args { get; set; }
      #endregion

      #region Methods
      public async Task<Status> Run(IProgress<FileRecord> progress, CancellationToken cancelToken)
      {
         Status = Status.working;
         return await Task.Run(() =>
         {
            var count = 10;
            List<Task<FileRecord>> tasks = new List<Task<FileRecord>>();
            if (!String.IsNullOrEmpty(Args))
            {
               bool success = int.TryParse(Args, out count);
            }
            for (int i = 0; i < count; i++)
            {
               tasks.Add(BuildAsyncTest(progress, i));
            }
            progress.Report(new FileRecord("Test", "Test", null));

            var results = Task.WhenAll(tasks);
            return Status.done;
         });
      }

      private Task<FileRecord> BuildAsyncTest(IProgress<FileRecord> progress, int index)
      {
         return Task.Run(() =>
         {
            var fr = new FileRecord($"{index} Source Test", $"{index} Dest Test", null);
            Thread.Sleep(Random.Next(100, 5000));
            progress.Report(fr);
            return fr;
         });
      }
      #endregion
   }
}
