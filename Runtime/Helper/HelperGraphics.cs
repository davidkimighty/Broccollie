using System;
using System.Collections;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.Helper
{
    public static partial class Helper
    {
        #region Color
        public static async Task ChangeColorGraduallyAsync(this MaskableGraphic graphic, Color targetColor, float duration, CancellationToken cancellationToken, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Color startColor = graphic.color;

            while (elapsedTime < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();
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

        public static IEnumerator ChangeColorGradually(this MaskableGraphic graphic, Color targetColor, float duration, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Color startColor = graphic.color;

            while (elapsedTime < duration)
            {
                if (graphic.color == targetColor) yield break;

                if (curve == null)
                    graphic.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                else
                    graphic.color = Color.Lerp(startColor, targetColor, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            graphic.color = targetColor;
            done?.Invoke();
        }

        #endregion
    }
}
