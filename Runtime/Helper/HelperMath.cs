using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.Helper
{
    public static partial class Helper
    {
        #region Vector3
        public static IEnumerator LerpPosition(this Transform transform, Vector3 targetPosition, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;

            while(elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingPosition, targetPosition, elapsedTime / duration);
                transform.position = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
            done?.Invoke();
        }

        public static IEnumerator LerpPosition(this Transform transform, Vector3 targetPosition, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingPosition, targetPosition, curve.Evaluate(elapsedTime / duration));
                transform.position = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
            done?.Invoke();
        }

        public static IEnumerator LerpLocalPosition(this Transform transform, Vector3 targetPosition, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingPosition, targetPosition, elapsedTime / duration);
                transform.localPosition = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = targetPosition;
            done?.Invoke();
        }

        public static IEnumerator LerpLocalPosition(this Transform transform, Vector3 targetPosition, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingPosition, targetPosition, curve.Evaluate(elapsedTime / duration));
                transform.localPosition = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = targetPosition;
            done?.Invoke();
        }

        public static IEnumerator LerpScale(this Transform transform, Vector3 targetScale, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingScale = transform.localScale;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingScale, targetScale, elapsedTime / duration);
                transform.localScale = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
            done?.Invoke();
        }

        public static IEnumerator LerpScale(this Transform transform, Vector3 targetScale, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingScale = transform.localScale;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingScale, targetScale, curve.Evaluate(elapsedTime / duration));
                transform.localScale = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
            done?.Invoke();
        }
        #endregion

        #region Quaternion
        public static IEnumerator LerpRotation(this Transform transform, Quaternion targetRotation, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.rotation;

            while (elapsedTime < duration)
            {
                Quaternion lerpValue = Quaternion.LerpUnclamped(startingRotation, targetRotation, elapsedTime / duration);
                transform.rotation = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRotation;
            done?.Invoke();
        }

        public static IEnumerator LerpRotation(this Transform transform, Quaternion targetRotation, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.rotation;

            while (elapsedTime < duration)
            {
                Quaternion lerpValue = Quaternion.LerpUnclamped(startingRotation, targetRotation, curve.Evaluate(elapsedTime / duration));
                transform.rotation = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRotation;
            done?.Invoke();
        }

        public static IEnumerator LerpLocalRotation(this Transform transform, Quaternion targetRotation, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.localRotation;

            while (elapsedTime < duration)
            {
                Quaternion lerpValue = Quaternion.LerpUnclamped(startingRotation, targetRotation, elapsedTime / duration);
                transform.localRotation = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localRotation = targetRotation;
            done?.Invoke();
        }

        public static IEnumerator LerpLocalRotation(this Transform transform, Quaternion targetRotation, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.localRotation;

            while (elapsedTime < duration)
            {
                Quaternion lerpValue = Quaternion.LerpUnclamped(startingRotation, targetRotation, curve.Evaluate(elapsedTime / duration));
                transform.localRotation = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localRotation = targetRotation;
            done?.Invoke();
        }
        #endregion

        #region Value
        public static IEnumerator<float> LerpValue(float startValue, float targetValue, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float lerpValue = Mathf.LerpUnclamped(startValue, targetValue, curve.Evaluate(elapsedTime / duration));
                elapsedTime += Time.deltaTime;
                yield return lerpValue;
            }
            yield return targetValue;
            done?.Invoke();
        }

        #endregion
    }
}
