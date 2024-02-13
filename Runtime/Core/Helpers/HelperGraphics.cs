using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Broccollie.Core
{
    public static partial class Helper
    {
        #region Color
        public static async Task LerpColorAsync(this MaskableGraphic graphic, Color targetColor, float duration, CancellationToken ct, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Color startColor = graphic.color;

            while (elapsedTime < duration)
            {
                if (ct.IsCancellationRequested)
                    ct.ThrowIfCancellationRequested();

                if (curve == null)
                    graphic.color = Color.LerpUnclamped(startColor, targetColor, elapsedTime / duration);
                else
                    graphic.color = Color.LerpUnclamped(startColor, targetColor, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            graphic.color = targetColor;
            done?.Invoke();
        }

        public static IEnumerator LerpColor(this MaskableGraphic graphic, Color targetColor, float duration, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Color startColor = graphic.color;

            while (elapsedTime < duration)
            {
                if (curve == null)
                    graphic.color = Color.LerpUnclamped(startColor, targetColor, elapsedTime / duration);
                else
                    graphic.color = Color.LerpUnclamped(startColor, targetColor, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            graphic.color = targetColor;
            done?.Invoke();
        }

        #endregion
    }
}
