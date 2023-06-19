using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Broccollie.Core
{
    public static partial class Helper
    {
        public static async Task LerpPositionAsync(this Transform transform, Vector3 targetPosition, float duration, CancellationToken ct, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;

            while (elapsedTime < duration)
            {
                if (ct.IsCancellationRequested)
                    ct.ThrowIfCancellationRequested();

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

        public static async Task LerpLocalPositionAsync(this Transform transform, Vector3 targetPosition, float duration, CancellationToken ct, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                if (ct.IsCancellationRequested)
                    ct.ThrowIfCancellationRequested();

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

        public static async Task LerpAnchoredPositionAsync(this RectTransform rectTransform, Vector2 targetPosition, float duration, CancellationToken ct, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector2 startingPosition = rectTransform.anchoredPosition;

            while (elapsedTime < duration)
            {
                if (ct.IsCancellationRequested)
                    ct.ThrowIfCancellationRequested();

                if (rectTransform.anchoredPosition == targetPosition) break;

                if (curve == null)
                    rectTransform.anchoredPosition = Vector2.LerpUnclamped(startingPosition, targetPosition, elapsedTime / duration);
                else
                    rectTransform.anchoredPosition = Vector2.LerpUnclamped(startingPosition, targetPosition, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                await Task.Yield();
            }
            rectTransform.anchoredPosition = targetPosition;
            done?.Invoke();
        }

        public static async Task LerpScaleAsync(this Transform transform, Vector3 targetScale, float duration, CancellationToken ct, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingScale = transform.localScale;

            while (elapsedTime < duration)
            {
                if (ct.IsCancellationRequested)
                    ct.ThrowIfCancellationRequested();

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

        public static async Task LerpRotationAsync(this Transform transform, Quaternion targetRotation, float duration, CancellationToken ct, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.rotation;

            while (elapsedTime < duration)
            {
                if (ct.IsCancellationRequested)
                    ct.ThrowIfCancellationRequested();

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

        public static async Task LerpLocalRotationAsync(this Transform transform, Quaternion targetRotation, float duration, CancellationToken ct, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.localRotation;

            while (elapsedTime < duration)
            {
                if (ct.IsCancellationRequested)
                    ct.ThrowIfCancellationRequested();

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

        public static IEnumerator LerpPosition(this Transform transform, Vector3 targetPosition, float duration, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;

            while (elapsedTime < duration)
            {
                if (transform.position == targetPosition) yield break;

                if (curve == null)
                    transform.position = Vector3.LerpUnclamped(startingPosition, targetPosition, elapsedTime / duration);
                else
                    transform.position = Vector3.LerpUnclamped(startingPosition, targetPosition, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
            done?.Invoke();
        }

        public static IEnumerator LerpLocalPosition(this Transform transform, Vector3 targetPosition, float duration, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                if (transform.localPosition == targetPosition) yield break;

                if (curve == null)
                    transform.localPosition = Vector3.LerpUnclamped(startingPosition, targetPosition, elapsedTime / duration);
                else
                    transform.localPosition = Vector3.LerpUnclamped(startingPosition, targetPosition, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = targetPosition;
            done?.Invoke();
        }

        public static IEnumerator LerpScale(this Transform transform, Vector3 targetScale, float duration, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Vector3 startingScale = transform.localScale;

            while (elapsedTime < duration)
            {
                if (transform.localScale == targetScale) yield break;

                if (curve == null)
                    transform.localScale = Vector3.LerpUnclamped(startingScale, targetScale, elapsedTime / duration);
                else
                    transform.localScale = Vector3.LerpUnclamped(startingScale, targetScale, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;
            done?.Invoke();
        }

        public static IEnumerator LerpRotation(this Transform transform, Quaternion targetRotation, float duration, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.rotation;

            while (elapsedTime < duration)
            {
                if (transform.rotation == targetRotation) yield break;

                if (curve == null)
                    transform.rotation = Quaternion.LerpUnclamped(startingRotation, targetRotation, elapsedTime / duration);
                else
                    transform.rotation = Quaternion.LerpUnclamped(startingRotation, targetRotation, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.rotation = targetRotation;
            done?.Invoke();
        }

        public static IEnumerator LerpLocalRotation(this Transform transform, Quaternion targetRotation, float duration, AnimationCurve curve = null, Action done = null)
        {
            float elapsedTime = 0f;
            Quaternion startingRotation = transform.localRotation;

            while (elapsedTime < duration)
            {
                if (transform.localRotation == targetRotation) yield break;

                if (curve == null)
                    transform.localRotation = Quaternion.LerpUnclamped(startingRotation, targetRotation, elapsedTime / duration);
                else
                    transform.localRotation = Quaternion.LerpUnclamped(startingRotation, targetRotation, curve.Evaluate(elapsedTime / duration));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localRotation = targetRotation;
            done?.Invoke();
        }
    }
}
