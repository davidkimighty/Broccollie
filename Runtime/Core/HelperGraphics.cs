using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Broccollie.Core
{
    public static partial class Helper
    {
        #region Color
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
