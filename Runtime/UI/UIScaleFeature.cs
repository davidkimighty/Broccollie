using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Core;
using CollieMollie.Helper;
using UnityEngine;

namespace CollieMollie.UI
{
    public class UIScaleFeature : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        [SerializeField] private List<Element> _elements = null;

        #endregion

        #region Public Functions
        public void Change(InteractionState state)
        {
            if (!_isEnabled) return;

            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                element.ChangeScale(this, state);
            }
        }

        #endregion

        [Serializable]
        public class Element
        {
            public bool IsEnabled = true;
            public Transform TargetObject = null;
            public UIScalePreset Preset = null;

            private IEnumerator _scaleChangeAction = null;

            public void ChangeScale(MonoBehaviour mono, InteractionState state)
            {
                if (_scaleChangeAction != null)
                    mono.StopCoroutine(_scaleChangeAction);

                UIScalePreset.ScaleState scaleState = Array.Find(Preset.ScaleStates, x => x.ExecutionState == state);
                if (!scaleState.IsValid())
                    scaleState = Array.Find(Preset.ScaleStates, x => x.ExecutionState == InteractionState.Default);

                if (scaleState.IsEnabled)
                {
                    if (!scaleState.IsEnabled) return;
                    _scaleChangeAction = TargetObject.LerpScale(Vector3.one * scaleState.TargetScale, scaleState.Duration, scaleState.Curve);
                    mono.StartCoroutine(_scaleChangeAction);
                }
            }
        }
    }
}
