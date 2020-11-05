using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMover.Control.Observers
{
   public interface IObservable
   {
      void AddObserver(IObserver observer);
      void RemoveObserver(IObserver observer);
      void Notify(FileRecord file);
   }
}
