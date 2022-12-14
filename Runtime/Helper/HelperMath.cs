using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CollieMollie.Helper
{
    public static partial class Helper
    {
        #region Vector3
        public static async Task LerpPositionAsync(this Transform transform, Vector3 targetPosition, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingPosition, targetPosition, elapsedTime / duration);
                transform.position = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.position = targetPosition;
            done?.Invoke();
        }

        public static async Task LerpPositionAsync(this Transform transform, Vector3 targetPosition, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingPosition, targetPosition, curve.Evaluate(elapsedTime / duration));
                transform.position = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.position = targetPosition;
            done?.Invoke();
        }

        public static async Task LerpLocalPositionAsync(this Transform transform, Vector3 targetPosition, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingPosition, targetPosition, elapsedTime / duration);
                transform.localPosition = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.localPosition = targetPosition;
            done?.Invoke();
        }

        public static async Task LerpLocalPositionAsync(this Transform transform, Vector3 targetPosition, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingPosition, targetPosition, curve.Evaluate(elapsedTime / duration));
                transform.localPosition = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.localPosition = targetPosition;
            done?.Invoke();
        }

        public static async Task LerpScaleAsync(this Transform transform, Vector3 targetScale, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingScale = transform.localScale;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingScale, targetScale, elapsedTime / duration);
                transform.localScale = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.localScale = targetScale;
            done?.Invoke();
        }

        public static async Task LerpScaleAsync(this Transform transform, Vector3 targetScale, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingScale = transform.localScale;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.LerpUnclamped(startingScale, targetScale, curve.Evaluate(elapsedTime / duration));
                transform.localScale = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.localScale = targetScale;
            done?.Invoke();
        }
        #endregion

        #region Quaternion
        public static async Task LerpRotationAsync(this Transform transform, Quaternion targetRotation, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.rotation;

            while (elapsedTime < duration)
            {
                Quaternion lerpValue = Quaternion.LerpUnclamped(startingRotation, targetRotation, elapsedTime / duration);
                transform.rotation = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.rotation = targetRotation;
            done?.Invoke();
        }

        public static async Task LerpRotationAsync(this Transform transform, Quaternion targetRotation, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.rotation;

            while (elapsedTime < duration)
            {
                Quaternion lerpValue = Quaternion.LerpUnclamped(startingRotation, targetRotation, curve.Evaluate(elapsedTime / duration));
                transform.rotation = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.rotation = targetRotation;
            done?.Invoke();
        }

        public static async Task LerpLocalRotationAsync(this Transform transform, Quaternion targetRotation, float duration, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.localRotation;

            while (elapsedTime < duration)
            {
                Quaternion lerpValue = Quaternion.LerpUnclamped(startingRotation, targetRotation, elapsedTime / duration);
                transform.localRotation = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.localRotation = targetRotation;
            done?.Invoke();
        }

        public static async Task LerpLocalRotationAsync(this Transform transform, Quaternion targetRotation, float duration, AnimationCurve curve, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.localRotation;

            while (elapsedTime < duration)
            {
                Quaternion lerpValue = Quaternion.LerpUnclamped(startingRotation, targetRotation, curve.Evaluate(elapsedTime / duration));
                transform.localRotation = lerpValue;
                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.localRotation = targetRotation;
            done?.Invoke();
        }
        #endregion
    }
}
