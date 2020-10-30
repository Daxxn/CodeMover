using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMover.Control.Strategies
{
   public interface IFileResults
   {
      List<FileRecord> Results { get; set; }
   }
}
