using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace CollieMollie.Helper
{
    public static partial class Helper
    {
        #region Transform
        public static async Task LerpPositionAsync(this Transform transform, Vector3 targetPosition, float duration, CancellationToken cancellationToken, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;

            while (elapsedTime < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (transform.position == targetPosition) break;

                if (curve == null)
                    transform.position = Vector3.LerpUnclamped(startingPosition, targetPosition, elapsedTime / duration);
                else
                    transform.position = Vector3.LerpUnclamped(startingPosition, targetPosition, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.position = targetPosition;
            done?.Invoke();
        }

        public static async Task LerpLocalPositionAsync(this Transform transform, Vector3 targetPosition, float duration, CancellationToken cancellationToken, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (transform.localPosition == targetPosition) break;

                if (curve == null)
                    transform.localPosition = Vector3.LerpUnclamped(startingPosition, targetPosition, elapsedTime / duration);
                else
                    transform.localPosition = Vector3.LerpUnclamped(startingPosition, targetPosition, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.localPosition = targetPosition;
            done?.Invoke();
        }

        public static async Task LerpScaleAsync(this Transform transform, Vector3 targetScale, float duration, CancellationToken cancellationToken, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingScale = transform.localScale;

            while (elapsedTime < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (transform.localScale == targetScale) break;

                if (curve == null)
                    transform.localScale = Vector3.LerpUnclamped(startingScale, targetScale, elapsedTime / duration);
                else
                    transform.localScale = Vector3.LerpUnclamped(startingScale, targetScale, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.localScale = targetScale;
            done?.Invoke();
        }

        public static async Task LerpRotationAsync(this Transform transform, Quaternion targetRotation, float duration, CancellationToken cancellationToken, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.rotation;

            while (elapsedTime < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (transform.rotation == targetRotation) break;

                if (curve == null)
                    transform.rotation = Quaternion.LerpUnclamped(startingRotation, targetRotation, elapsedTime / duration);
                else
                    transform.rotation = Quaternion.LerpUnclamped(startingRotation, targetRotation, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.rotation = targetRotation;
            done?.Invoke();
        }

        public static async Task LerpLocalRotationAsync(this Transform transform, Quaternion targetRotation, float duration, CancellationToken cancellationToken, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.localRotation;

            while (elapsedTime < duration)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (transform.localRotation == targetRotation) break;

                if (curve == null)
                    transform.localRotation = Quaternion.LerpUnclamped(startingRotation, targetRotation, elapsedTime / duration);
                else
                    transform.localRotation = Quaternion.LerpUnclamped(startingRotation, targetRotation, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            transform.localRotation = targetRotation;
            done?.Invoke();
        }

        #endregion

        #region Quaternion
        public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
        {
            if (Quaternion.Dot(a, b) < 0)
                return a * Quaternion.Inverse(Multiply(b, -1f));
            return a * Quaternion.Inverse(b);
        }

        public static Quaternion Multiply(Quaternion q, float scalar)
        {
            return new Quaternion(q.x * scalar, q.y * scalar, q.z * scalar, q.w * scalar);
        }

        #endregion
    }
}
