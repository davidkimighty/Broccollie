using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CollieMollie.Core;
using CollieMollie.Helper;
using CollieMollie.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITransformFeature : MonoBehaviour, IUIFeature
{
    #region Variable Field
    [SerializeField] private bool _isEnabled = true;
    [SerializeField] private List<Element> _elements = null;

    private Operation _featureOperation = new Operation();

    #endregion

    #region Public Functions
    public void Execute(string state, PointerEventData eventData = null, Action done = null)
    {
        if (!_isEnabled) return;

        _featureOperation.Stop(this);

        List<float> durations = new List<float>();
        foreach (Element element in _elements)
        {
            if (!element.IsEnabled) continue;
            _featureOperation.Add(element.ChangeTransform(this, state));
            durations.Add(element.Preset.GetDuration(state));
        }

        _featureOperation.Start(this, durations.Count > 0 ? durations.Max() : 0, done);
    }

    #endregion

    [Serializable]
    public class Element
    {
        public bool IsEnabled = true;
        public Transform TargetObject = null;
        public UITransformPreset Preset = null;

        private Operation _transformOperation = new Operation();

        public IEnumerator ChangeTransform(MonoBehaviour mono, string state)
        {
            UITransformPreset.Setting setting = Array.Find(Preset.States, x => x.ExecutionState.ToString() == state);
            if (Preset.IsValid(setting.ExecutionState) && setting.IsEnabled)
            {
                if (!setting.IsEnabled) yield break;

                _transformOperation.Stop(mono);

                if (setting.PositionSettingEnabled)
                    _transformOperation.Add(TargetObject.LerpPosition(setting.TargetPosition.position, setting.PositionSettingDuration, setting.PositionSettingCurve));

                if (setting.ScaleSettingEnabled)
                    _transformOperation.Add(TargetObject.LerpScale(Vector3.one * setting.TargetScale, setting.ScaleSettingDuration, setting.ScaleSettingCurve));

                _transformOperation.Start(mono);
            }
        }
    }
}
