using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace CollieMollie.Game
{
    public class CameraAutoFocus : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private float _focusRayRadius = 1f;
        [SerializeField] private float _focusRayLength = 100f;
        [SerializeField] private float _focusDuration = 0.6f;
        [SerializeField] private AnimationCurve _focusCurve = null;

        private float _hitDst = 0f;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        #endregion

        #region Public Functions
        public async Task Focus(Transform cam, DepthOfField dof)
        {
            

            Ray ray = new Ray(cam.position, cam.forward);
            if (Physics.SphereCast(ray, _focusRayRadius, out RaycastHit rayHit, _focusRayLength))
            {
                float hitDst = Vector3.Distance(cam.position, rayHit.point);
                if (_hitDst == hitDst) return;
                _hitDst = hitDst;

                _cts.Cancel();
                _cts = new CancellationTokenSource();

                float elapsedTime = 0f;
                while (elapsedTime <= _focusDuration)
                {
                    _cts.Token.ThrowIfCancellationRequested();

                    dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, _hitDst, _focusCurve.Evaluate(elapsedTime / _focusDuration));
                    elapsedTime += Time.deltaTime;
                    await Task.Yield();
                }
                dof.focusDistance.value = _hitDst;
            }
        }

        #endregion
    }
}
