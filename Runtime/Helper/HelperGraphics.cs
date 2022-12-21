using System;
using System.Collections;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace CollieMollie.Helper
{
    public static partial class Helper
    {
        #region Color
        public static async Task ChangeColorGraduallyAsync(this MaskableGraphic graphic, Color targetColor, float duration, AnimationCurve curve = null, CancellationTokenSource tokenSource = null, Action done = null)
        {
            float elapsedTime = 0f;
            Color startColor = graphic.color;

            while (elapsedTime < duration)
            {
                if (tokenSource != null)
                    tokenSource.Token.ThrowIfCancellationRequested();
                if (graphic.color == targetColor) break;

                if (curve == null)
                    graphic.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                else
                    graphic.color = Color.Lerp(startColor, targetColor, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            graphic.color = targetColor;
            done?.Invoke();
        }

        #endregion
    }
}
