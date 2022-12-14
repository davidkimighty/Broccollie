using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CollieMollie.Helper
{
    public static partial class Helper
    {
        #region Color
        public static async Task ChangeColorGraduallyAsync(this MaskableGraphic graphic, Color targetColor, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Color startColor = graphic.color;

            while (elapsedTime < duration)
            {
                if (graphic.color == targetColor) return;

                graphic.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            graphic.color = targetColor;
            done?.Invoke();
        }

        public static async Task ChangeColorGraduallyAsync(this MaskableGraphic graphic, Color targetColor, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Color startColor = graphic.color;

            while (elapsedTime < duration)
            {
                if (graphic.color == targetColor) return;

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
