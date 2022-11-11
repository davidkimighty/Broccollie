using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CollieMollie.Helper
{
    public static partial class Helper
    {
        #region Layer
        public static void ChangeLayersRecursive(this Transform root, int layer)
        {
            root.gameObject.layer = layer;
            foreach (Transform child in root)
                ChangeLayersRecursive(child, layer);
        }
        #endregion

        #region Diagnostics
        public static Stopwatch StartWatch()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            return stopWatch;
        }

        public static TimeSpan StopWatch(this Stopwatch watch, out string timeText)
        {
            watch.Stop();
            TimeSpan stopTime = watch.Elapsed;
            timeText = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                stopTime.Hours, stopTime.Minutes, stopTime.Seconds, stopTime.Milliseconds / 10); ;
            return watch.Elapsed;
        }
        #endregion
    }
}
