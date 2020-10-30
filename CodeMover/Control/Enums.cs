using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMover.Control
{
   public enum Command
   {
      exit = 1,
      run = 2,
      src = 3,
      dest = 4,
      settings = 5,
      invalid = 0,
   };
   public enum Status
   {
      working,
      ready,
      done,
   };
   public enum Accessor
   {
      source,
      destination,
      exclude,
      files,
      folders,
   };
   public enum Action
   {
      add,
      rem,
   };
}
