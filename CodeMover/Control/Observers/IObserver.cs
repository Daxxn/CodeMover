using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMover.Control.Observers
{
   public interface IObserver
   {
      void Update(FileRecord completedFile);
   }
}
