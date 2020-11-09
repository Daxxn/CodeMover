using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace CodeMover.Control
{
   public class SimpleAnimation
   {
      #region - Fields & Properties
      private readonly string[] _frames = { "\\", "|", "/", "-" };
      private int currentFrame { get; set; } = 0;
      private readonly uint _timeout = 60;
      private System.Timers.Timer _timer { get; set; }
      #endregion

      #region - Constructors
      public SimpleAnimation() { }
      public SimpleAnimation(uint timeout) => _timeout = timeout;
      public SimpleAnimation(IEnumerable<string> frames)
      {
         _frames = frames.ToArray();
      }
      public SimpleAnimation(IEnumerable<string> frames, uint timeout)
      {
         _frames = frames.ToArray();
         _timeout = timeout;
      }
      #endregion

      #region - Methods
      public void Next()
      {
         NextFrame();
         Thread.Sleep((int)_timeout);
      }

      private void NextFrame()
      {
         Console.CursorVisible = false;
         string nextFrame = $"Working {_frames[currentFrame]}";
         Console.SetCursorPosition(
            Console.WindowWidth - (nextFrame.Length + 1),
            Console.CursorTop > 0 ? Console.CursorTop - 1 : 0
         );
         Console.WriteLine(nextFrame);
         if (currentFrame++ >= (_frames.Length - 1))
         {
            currentFrame = 0;
         }
         Console.CursorVisible = true;
      }

      public void Start()
      {
         _timer = new System.Timers.Timer(_timeout);
         _timer.Start();
         _timer.Elapsed += _timer_Elapsed;
      }

      public void End()
      {
         _timer.Stop();
         _timer = null;
      }

      private void _timer_Elapsed(object sender, ElapsedEventArgs e)
      {
         NextFrame();
      }
      #endregion

      #region - Full Properties

      #endregion
   }
}
