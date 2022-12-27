using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CollieMollie.Helper;
using CollieMollie.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITransformFeature : BaseUIFeature
{
    #region Variable Field
    [SerializeField] private bool _isEnabled = true;
    [SerializeField] private List<Element> _elements = null;

    #endregion

    #region Public Functions
    public override async Task ExecuteAsync(string state, CancellationToken cancellationToken, Action done = null)
    {
        if (!_isEnabled) return;

        List<Task> executions = new List<Task>();
        foreach (Element element in _elements)
        {
            if (!element.IsEnabled) continue;

            UITransformPreset.Setting setting = Array.Find(element.Preset.States, x => x.ExecutionState.ToString() == state);
            if (IsValid(setting.ExecutionState) && setting.IsEnabled)
            {
                executions.Add(element.ChangePosition(state, setting, cancellationToken));
                executions.Add(element.ChangeScale(state, setting, cancellationToken));
            }
        }
        await Task.WhenAll(executions);
        done?.Invoke();
    }

    #endregion

    [Serializable]
    public class Element
    {
        public bool IsEnabled = true;
        public Transform TargetObject = null;
        public UITransformPreset Preset = null;

        public async Task ChangePosition(string state, UITransformPreset.Setting setting, CancellationToken cancellationToken)
        {
            if (setting.PositionSettingEnabled)
                await TargetObject.LerpPositionAsync(setting.TargetPosition.position, setting.PositionSettingDuration, cancellationToken, setting.PositionSettingCurve);
        }

        public async Task ChangeScale(string state, UITransformPreset.Setting setting, CancellationToken cancellationToken)
        {
            if (setting.ScaleSettingEnabled)
                await TargetObject.LerpScaleAsync(Vector3.one * setting.TargetScale, setting.ScaleSettingDuration, cancellationToken, setting.ScaleSettingCurve);
        }
    }
}
