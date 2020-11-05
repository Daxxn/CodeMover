using CodeMover.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMover.Control.Observers
{
   public class FileObservable : IObservable
   {
      readonly List<IObserver> observers = new List<IObserver>();

      public void Notify(FileRecord file)
      {
         foreach (var observer in observers)
         {
            observer.Update(file);
         }
      }

      public void AddObserver(IObserver observer)
      {
         observers.Add(observer);
      }

      public void RemoveObserver(IObserver observer)
      {
         observers.Remove(observer);
      }
   }
}
