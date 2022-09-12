using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieMollie.Helper
{
    public static partial class Helper
    {
        #region Vector3
        public static IEnumerator LerpPosition(this Transform transform, Vector3 targetPosition, float duration)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.position;

            while(elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
                transform.position = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;
        }

        public static IEnumerator LerpLocalPosition(this Transform transform, Vector3 targetPosition, float duration)
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = transform.localPosition;

            while (elapsedTime < duration)
            {
                Vector3 lerpValue = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
                transform.localPosition = lerpValue;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localPosition = targetPosition;
        }
        #endregion

        #region Quaternion

        #endregion
    }
}
