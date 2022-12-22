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

        #region Camera Viewport
        public static Vector3 ViewportToWorldPlanePoint(this Camera cam, float zDepth, Vector2 viewportCord)
        {
            Vector2 angles = cam.ViewportPointToAngle(viewportCord);
            float xOffset = Mathf.Tan(angles.x) * zDepth;
            float yOffset = Mathf.Tan(angles.y) * zDepth;
            Vector3 cameraPlanePosition = new Vector3(xOffset, yOffset, zDepth);
            cameraPlanePosition = cam.transform.TransformPoint(cameraPlanePosition);
            return cameraPlanePosition;
        }

        public static Vector2 ViewportPointToAngle(this Camera cam, Vector2 ViewportCord)
        {
            float adjustedAngle = AngleProportion(cam.fieldOfView / 2, cam.aspect) * 2;
            float xProportion = (ViewportCord.x - 0.5f) / 0.5f;
            float yProportion = (ViewportCord.y - 0.5f) / 0.5f;
            float xAngle = AngleProportion(adjustedAngle / 2, xProportion) * Mathf.Deg2Rad;
            float yAngle = AngleProportion(cam.fieldOfView / 2, yProportion) * Mathf.Deg2Rad;
            return new Vector2(xAngle, yAngle);
        }

        public static float AngleProportion(float angle, float proportion)
        {
            float oppisite = Mathf.Tan(angle * Mathf.Deg2Rad);
            float oppisiteProportion = oppisite * proportion;
            return Mathf.Atan(oppisiteProportion) * Mathf.Rad2Deg;
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
